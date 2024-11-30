using Mailjet.Client;
using Mailjet.Client.Resources;
using System;
using System.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Mailjet.Client.TransactionalEmails;
using Microsoft.AspNet.Identity;

public class EmailService : IIdentityMessageService
{
    public async Task SendAsync(IdentityMessage message)
    {
        await configMailjetAsync(message);
    }

    private async Task configMailjetAsync(IdentityMessage message)
    {
        MailjetClient client = new MailjetClient(
            ConfigurationManager.AppSettings["MJ_APIKEY_PUBLIC"],
            ConfigurationManager.AppSettings["MJ_APIKEY_PRIVATE"]);

        var email = new TransactionalEmailBuilder()
            .WithFrom(new SendContact("your-email@example.com"))
            .WithSubject(message.Subject)
            .WithHtmlPart(message.Body)
            .WithTextPart(message.Body)
            .WithTo(new SendContact(message.Destination))
            .Build();

        var response = await client.SendTransactionalEmailAsync(email);

        if (response.Messages.Length==1)
        {
            System.Diagnostics.Trace.TraceInformation($"Email sent successfully!");
        }
        else
        {
            System.Diagnostics.Trace.TraceError($"Failed to send email. {response.Messages[1]}");
        }
    }
}
