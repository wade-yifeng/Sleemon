using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Sleemon.Portal.Startup))]
namespace Sleemon.Portal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
