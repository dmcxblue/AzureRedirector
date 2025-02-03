using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Redirector.API
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
            // Add minimal required services (controllers if needed in the future)
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            // Redirect HTTP to HTTPS for secure communication
            app.UseHttpsRedirection();

            // Route all incoming requests through the Reverse Proxy logic
            app.Run(ReverseProxy.Invoke);
        }
    }
}
