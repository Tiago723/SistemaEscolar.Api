using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace SistemaEscolar.Api.Ioc
{
    public static class HealthCheck
    {
        public static void AddHealthCheckUri(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            //serviceCollection.AddHealthChecks();
            serviceCollection.AddHealthChecks()
            /*  Exemplo Check URLs:
                .AddUrlGroup(new Uri(configuration.GetSection("Links:HealthCheckSsp").Value), "Georeferenciamento.SSP", tags: new[] { "uri" })
                Exemplo Check DB:
                .AddSqlServer(configuration.GetConnectionString("SIOPMCRP"),name: "sqlserver", tags: new string[] { "db", "data" })
            */
            ;

            serviceCollection.AddHealthChecksUI()
                .AddInMemoryStorage();
        }

        public static void ConfigurarHealthCheckEndPoint(this WebApplication app)
        {

            app.UseHealthChecks("/healthChecking", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecks("/healthchecks-data-ui", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            // Activate the dashboard for UI
            app.UseHealthChecksUI(options =>
            {
                options.UIPath = "/monitor";
            });
        }
    }
}
