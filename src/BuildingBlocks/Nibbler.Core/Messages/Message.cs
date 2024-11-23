namespace Nibbler.Core.Messages;

public abstract class Message
{
    public string MessageType { get; set; }

    public Guid AggregateId { get; set; }

    public Message()
    {
        MessageType = GetType().Name; 
    }
}
