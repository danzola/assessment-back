namespace CoE.Assessment.Domain.Bus
{
    public interface IHandler<in T>
    {
        Task Handle(T message);
    }
}
