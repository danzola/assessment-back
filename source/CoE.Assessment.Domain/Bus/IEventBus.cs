using CoE.Assessment.Domain.Commands;
using CoE.Assessment.Domain.Events;

namespace CoE.Assessment.Domain.Bus
{
    interface IEventBus
    {
        Task SendCommand<T>(T command) where T : Command;
        Task Publish<T>(T @event) where T : Event;
        Task Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>;
    }
}
