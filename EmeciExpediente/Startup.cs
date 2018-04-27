using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EmeciExpediente.Startup))]
namespace EmeciExpediente
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
