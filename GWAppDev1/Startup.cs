using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GWAppDev1.Startup))]
namespace GWAppDev1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
