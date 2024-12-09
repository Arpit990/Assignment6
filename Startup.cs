using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VS2247A5.Startup))]
namespace VS2247A5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
