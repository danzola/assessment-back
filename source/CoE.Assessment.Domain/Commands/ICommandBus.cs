namespace CoE.Assessment.Domain.Commands
{
    public interface ICommandBus
    {
        Task SendCommand<T>(T command) where T : Command;
        Task SubscribeCommand<T, TH>()
            where T : Command
            where TH : ICommandHandler<T>;
    }
}
