using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NosTRAILgic.Startup))]
namespace NosTRAILgic
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
