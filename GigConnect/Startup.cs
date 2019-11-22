using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GigConnect.Startup))]
namespace GigConnect
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
