using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JS.Core.Utils;
using JS.MessageBus;

namespace JS.Contas.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"));
        }
    }
}