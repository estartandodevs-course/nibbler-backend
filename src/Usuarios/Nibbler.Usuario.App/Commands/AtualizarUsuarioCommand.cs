using Nibbler.Core.Messages;

namespace Nibbler.Usuario.App.Commands;

public class AtualizarUsuarioCommand : Command
{
    public Guid UsuarioId { get; private set; }
    public string Nome { get; private set; }
    public DateTime DataDeNascimento { get; private set; }
    public string Foto { get; private set; }

    public AtualizarUsuarioCommand(Guid usuarioId, string nome, DateTime dataDeNascimento, string foto)
    {
        UsuarioId = usuarioId;
        DataDeNascimento = dataDeNascimento;
        Foto = foto;
    }
}
