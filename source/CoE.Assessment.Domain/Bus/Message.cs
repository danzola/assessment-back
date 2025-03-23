namespace CoE.Assessment.Domain.Bus
{
    public abstract class Message
    {
        public Guid Id { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string MessageType { get; protected set; }

        protected Message()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            MessageType = GetType().Name;
        }
    }
}
