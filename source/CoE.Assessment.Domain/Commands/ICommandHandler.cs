using CoE.Assessment.Domain.Bus;

namespace CoE.Assessment.Domain.Commands
{
    public interface ICommandHandler<in TCommand> : IHandler<TCommand> where TCommand : Command
    {
    }
}
