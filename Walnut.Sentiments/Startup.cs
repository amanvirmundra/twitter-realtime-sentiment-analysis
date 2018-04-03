using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Walnut.Sentiments.Startup))]
namespace Walnut.Sentiments
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
