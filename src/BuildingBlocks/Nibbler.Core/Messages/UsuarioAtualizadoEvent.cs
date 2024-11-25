namespace Nibbler.Core.Messages;

public class UsuarioAtualizadoEvent : Event
{
    public Guid UsuarioId { get; private set; }

    public string Nome { get; private set; }

    public string Foto { get; private set; }

    public UsuarioAtualizadoEvent(Guid usuarioId, string nome, string foto)
    {
        UsuarioId = usuarioId;
        Nome = nome;
        Foto = foto;
    }
}