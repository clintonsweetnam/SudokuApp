using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sudoku.Helpers;
using Sudoku.Types.Configuration;

namespace Sudoku.Startups
{
    public abstract class StartupBase
    {
        public IConfigurationRoot Configuration { get; protected set; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.Configure<Configuration>(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseWebSockets();

            app.Use(async (context, next) =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    await (new SocketHelper().HandleConnection(webSocket));
                }
                else
                {
                    await next();
                }
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
