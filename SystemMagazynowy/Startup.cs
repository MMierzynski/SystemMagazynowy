using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SystemMagazynowy.Startup))]
namespace SystemMagazynowy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
