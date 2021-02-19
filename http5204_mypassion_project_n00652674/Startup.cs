using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(http5204_mypassion_project_n00652674.Startup))]
namespace http5204_mypassion_project_n00652674
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
