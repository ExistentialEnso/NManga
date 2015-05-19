using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NManga.Startup))]
namespace NManga
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
