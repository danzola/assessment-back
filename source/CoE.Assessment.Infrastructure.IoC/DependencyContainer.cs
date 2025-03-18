using CoE.Assessment.Domain.Bus;
using CoE.Assessment.Infrastructure.Bus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CoE.Assessment.Infrastructure.IoC
{
    public static class DependencyContainer
    {
        public static void AddMicroRabbitServices(this IServiceCollection services)
        {
            services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
            {
                var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMQBus(sp.GetRequiredService<IMediator>(), serviceScopeFactory);
            });
        }
    }
}
