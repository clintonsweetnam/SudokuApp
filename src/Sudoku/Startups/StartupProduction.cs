using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sudoku.Helpers;
using Sudoku.Interfaces.Logging;
using Sudoku.Logging;

namespace Sudoku.Startups
{
    public class StartupProduction : StartupBase
    {
        private readonly string _environment;

        public StartupProduction(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.Production.json", optional: true)
                .AddEnvironmentVariables();

            _environment = env.EnvironmentName;

            Configuration = builder.Build();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddTransient<IAsyncLogger, LogglyLogger>();
        }

        public override void Configure(IApplicationBuilder app)
        {
            base.Configure(app);

            app.UseExceptionHandler("/Home/Error");
        }
    }
}
