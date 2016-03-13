using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MassMineSweeper.Startup))]
namespace MassMineSweeper
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
