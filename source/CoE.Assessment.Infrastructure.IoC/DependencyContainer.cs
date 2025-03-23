using CoE.Assessment.Domain.Commands;
using CoE.Assessment.Domain.Events;
using CoE.Assessment.Infrastructure.Bus;
using Microsoft.Extensions.DependencyInjection;

namespace CoE.Assessment.Infrastructure.IoC
{
    public static class DependencyContainer
    {
        public static void AddMicroRabbitServices(this IServiceCollection services)
        {
            services.AddSingleton(sp =>
            {
                var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMQBus(serviceScopeFactory);
            });
            services.AddSingleton<IEventBus>(sp => sp.GetRequiredService<RabbitMQBus>());
            services.AddSingleton<ICommandBus>(sp => sp.GetRequiredService<RabbitMQBus>());
        }
    }
}
