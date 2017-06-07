using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Inward_Tapal.Startup))]
namespace Inward_Tapal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
