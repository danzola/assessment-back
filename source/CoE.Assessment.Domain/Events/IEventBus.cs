namespace CoE.Assessment.Domain.Events
{
    public interface IEventBus
    {
        Task PublishEvent<T>(T @event) where T : Event;
        Task SubscribeEvent<T, TH>()
            where T : Event
            where TH : IEventHandler<T>;
    }
}
