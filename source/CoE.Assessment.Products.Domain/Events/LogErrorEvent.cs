using CoE.Assessment.Domain.Events;

namespace CoE.Assessment.Products.Domain.Events
{
    public class LogErrorEvent(string message): Event
    {
        public string Message { get; init; } = message;
    }
}
