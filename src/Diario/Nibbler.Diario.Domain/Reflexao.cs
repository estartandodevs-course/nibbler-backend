using Nibbler.Core.DomainObjects;

namespace Nibbler.Diario.Domain;

public class Reflexao : Entity
{
    public string Conteudo { get; private set; }
    public DateTime DataDeCadastro { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Guid? EmocaoId { get; set; }

    // Navegações
    // public virtual Usuario Usuario { get; private set; }
    public virtual Emocao Emocao { get; private set; }

    protected Reflexao() { }

    public Reflexao(Guid usuarioId, string conteudo, Guid? emocaoId = null)
    {
        UsuarioId = usuarioId;
        Conteudo = conteudo;
        EmocaoId = emocaoId;
        DataDeCadastro = DateTime.Now;
    }

    public void AtualizarConteudo(string conteudo)
    {
        Conteudo = conteudo;
    }
}