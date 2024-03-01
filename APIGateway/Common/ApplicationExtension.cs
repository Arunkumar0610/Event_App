using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Common
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig=> 
            {
                var address = configuration["Consul:ConsulAddress"];
                consulConfig.Address = new Uri(address);
            }));
            return services;
        }

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app,IConfiguration configuration) 
        {
            var consulClient=app.ApplicationServices.GetRequiredService<IConsulClient>();
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger(configuration["Consul:ServiceName"]);
            var lifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();

            var registration = new AgentServiceRegistration()
            {
                ID = configuration["Consul:ServiceId"],
                Name = configuration["Consul:ServiceName"],
                Address = configuration["Consul:ServiceHost"],
                Port = Convert.ToInt32(configuration["Consul:ServicePort"])
            };

            logger.LogInformation("Registering with Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            consulClient.Agent.ServiceRegister(registration).ConfigureAwait(true);

            lifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("Un registering from Consul");
            });
            return app;
        }

        
    }
}