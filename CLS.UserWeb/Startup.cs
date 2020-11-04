using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CLS.UserWeb.Startup))]

namespace CLS.UserWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
