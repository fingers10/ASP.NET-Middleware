using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASP.NET_Middleware
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //ASP.NET Core allows for modular building of HTTP request pipeline using middlewares.
            //This is configured by default in startup and the order in which they run are important
            //3 types of middleware
            //1. Run - execute the delegate and terminates processing
            //2. Use - execute the delegate and proceed to next in pipeline
            //3. Map - conditionally execute a method and doesnot return to pipeline
            //Requests are processed from top to bottom and outside to inside and back again
            //These are created once in startup

            //app.UseMiddleware<ExceptionMiddleware>();

            //2. Use - used to log browser information or any other request logs
            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("Hello from Use Middleware <br/>");
            //    await next();
            //    await context.Response.WriteAsync("Bye from Use Middleware <br/>");
            //});

            //3. Map
            //app.Map("/branch", HandleBranch);

            //app.MapWhen(context => context.Request.Query.ContainsKey("query"), context =>
            //{
            //    app.Run(async (context) =>
            //    {
            //        await context.Response.WriteAsync("Hello from inside MapWhen Middleware <br/>");
            //    });
            //});

            //1. Run - should always be the last one in pipeline
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello from Run Middleware <br/>");
            });
        }

        private static void HandleBranch(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Hello from inside Map Use Middleware <br/>");
                await next();
                await context.Response.WriteAsync("Bye from inside Map Use Middleware <br/>");
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello from inside Map Run Middleware <br/>");
            });
        }
    }
}
