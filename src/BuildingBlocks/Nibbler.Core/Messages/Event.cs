using MediatR;

namespace Nibbler.Core.Messages;

public abstract class Event : Message, INotification
{
    public DateTime Timestamp { get; private set;}

    public Event()
    {
        Timestamp = DateTime.UtcNow;
    }
}
