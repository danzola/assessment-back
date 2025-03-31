using CoE.Assessment.Domain.Events;
using CoE.Assessment.Logs.Domain.Events;
using Microsoft.Extensions.Logging;

namespace CoE.Assessment.Logs.Domain.EventHandlers
{
    public class LogErrorEventHandler(ILogger<LogErrorEventHandler> logger) : IEventHandler<LogErrorEvent>
    {
        private readonly ILogger<LogErrorEventHandler> _logger = logger;

        public async Task Handle(LogErrorEvent message)
        {
            _logger.LogError("{Message}", message.Message);
            await Task.CompletedTask;
        }
    }
}
