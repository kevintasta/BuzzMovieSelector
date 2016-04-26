using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BuzzMovieSelector.Startup))]
namespace BuzzMovieSelector
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
