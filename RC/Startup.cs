using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RC.Startup))]
namespace RC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
