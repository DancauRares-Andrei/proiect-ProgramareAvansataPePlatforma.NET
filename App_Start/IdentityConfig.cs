using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Mailjet.Client.TransactionalEmails;
using Mailjet.Client;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using proiect_ProgramareAvansataPePlatforma.NET.Models;
using Newtonsoft.Json.Linq;
using Mailjet.Client.Resources;
using System.Net.Mail;
using System.Net;
using System.IO;
using Microsoft.Extensions.Configuration;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace proiect_ProgramareAvansataPePlatforma.NET
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await configSmtpAsync(message);
        }

        private async Task configSmtpAsync(IdentityMessage message)
        {
            try
            {
                var smtpHost = ConfigurationManager.AppSettings["SmtpHost"];
                var smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
                var enableSsl = bool.Parse(ConfigurationManager.AppSettings["SmtpEnableSsl"]);
                var builder = new ConfigurationBuilder()
                .SetBasePath(@"C:\Users\Andrei Rares\source\repos\proiect-ProgramareAvansataPePlatforma.NET")
                .AddJsonFile("localsettings.json", optional: true, reloadOnChange: true);
                IConfigurationRoot configuration = builder.Build();
                var smtpUserName = configuration["SmtpEmail"];
                var smtpPassword = configuration["SmtpPassword"];
                // Logarea valorilor pentru a verifica dacă sunt corect preluate
                System.Diagnostics.Trace.TraceInformation($"SMTP Host: {smtpHost}");
                System.Diagnostics.Trace.TraceInformation($"SMTP Port: {smtpPort}");
                System.Diagnostics.Trace.TraceInformation($"Enable SSL: {enableSsl}");
                System.Diagnostics.Trace.TraceInformation($"SMTP Email: {smtpUserName}");
                System.Diagnostics.Trace.TraceInformation($"SMTP Password: {(string.IsNullOrEmpty(smtpPassword) ? "Not Set" : "Set")}");

                if (string.IsNullOrEmpty(smtpUserName) || string.IsNullOrEmpty(smtpPassword))
                {
                    throw new ArgumentNullException("SMTP_EMAIL or SMTP_PASSWORD", "SMTP credentials are not set correctly.");
                }

                var smtpClient = new SmtpClient(smtpHost, smtpPort)
                {
                    EnableSsl = enableSsl,
                    Credentials = new NetworkCredential(smtpUserName, smtpPassword)
                };

                var mailMessage = new MailMessage(smtpUserName, message.Destination)
                {
                    Subject = message.Subject,
                    Body = message.Body,
                    From = new MailAddress(smtpUserName),
                    IsBodyHtml = true // Setează la true dacă emailul conține HTML
                };

                await smtpClient.SendMailAsync(mailMessage);

                System.Diagnostics.Trace.TraceInformation("Email sent successfully!");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Failed to send email: {ex.Message}");
            }
        }

    }


    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
