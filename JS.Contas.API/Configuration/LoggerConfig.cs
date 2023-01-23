using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace JS.Contas.API.Configuration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddElmahIo(o =>
            {
                o.ApiKey = "9976c8dd746544c488018036da12e911";
                o.LogId = new Guid("817d89d5-02e5-43ac-b30a-845dab85794f");
            });

            services.AddHealthChecks()
                            .AddElmahIoPublisher(options =>
                            {
                                options.ApiKey = "9976c8dd746544c488018036da12e911";
                                options.LogId = new Guid("817d89d5-02e5-43ac-b30a-845dab85794f");
                                options.HeartbeatId = "API Produtos";

                            })
                            .AddCheck("Produtos", new SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection")))
                            .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), name: "BancoSQL");

            services.AddHealthChecksUI()
                .AddSqlServerStorage(configuration.GetConnectionString("DefaultConnection")); ;


            return services;
        }
        public static IApplicationBuilder UserLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();
            return app;
        }
    }
}