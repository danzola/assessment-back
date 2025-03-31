using CoE.Assessment.Domain.Events;

namespace CoE.Assessment.Logs.Domain.Events
{
    public class LogErrorEvent(string message): Event
    {
        public string Message { get; init; } = message;
    }
}
