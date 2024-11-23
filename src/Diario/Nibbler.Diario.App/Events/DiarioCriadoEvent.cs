using Nibbler.Core.Messages;

namespace Nibbler.Diario.app.Events;

public class DiarioCriadoEvent : Event
{
    public Guid DiarioId { get; private set; }
    public Guid UsuarioId { get; private set; }
    public DateTime DataCriacao { get; private set; }

    public DiarioCriadoEvent(Guid diarioId, Guid usuarioId)
    {
        DiarioId = diarioId;
        UsuarioId = usuarioId;
        DataCriacao = DateTime.Now;
    }
}