using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProxyKit;

namespace ProxyApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddProxy();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.RunProxy(async context =>
            {
                context.Request.QueryString = context
                                              .Request
                                              .QueryString
                                              .Add("method", "flickr.photos.search")
                                              .Add("api_key", "e323e04846805cdcaca5e8cfbd341967")
                                              .Add("format", "rest");

                var result = await context
                                   .ForwardTo("https://api.flickr.com/services/rest/")
                                   .Execute();

                return result;
            });
        }
    }
}
