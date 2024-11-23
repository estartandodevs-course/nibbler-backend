using Nibbler.Core.DomainObjects;

namespace Nibbler.Diario.Domain;

public class Reflexao : Entity
{
    public Usuario Usuario { get; private set; }
    public string Conteudo { get; private set; }
    public DateTime DataDeCadastro { get; private set; }
    public string Descricao { get; private set; }

    // EF Constructor
    protected Reflexao() { }

    public Reflexao(Usuario usuario, string conteudo)
    {
        Usuario = usuario;
        Conteudo = conteudo;
        DataDeCadastro = DateTime.Now;
    }
}