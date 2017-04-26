using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(x265Com.Startup))]
namespace x265Com
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
