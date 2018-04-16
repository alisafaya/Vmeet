using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Vmeet.Startup))]
namespace Vmeet
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
