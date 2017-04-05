using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Test_IGuide.Startup))]
namespace Test_IGuide
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
