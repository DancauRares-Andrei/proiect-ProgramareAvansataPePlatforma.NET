using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(proiect_ProgramareAvansataPePlatforma.NET.Startup))]
namespace proiect_ProgramareAvansataPePlatforma.NET
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
