using CoE.Assessment.Domain.Bus;

namespace CoE.Assessment.Domain.Events
{
    public interface IEventHandler<in TEvent> : IHandler<TEvent> where TEvent : Event
    {
    }    
}
