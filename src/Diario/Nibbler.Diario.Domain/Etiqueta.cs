using Nibbler.Core.DomainObjects;
namespace Nibbler.Diario.Domain;

public class Etiqueta : Entity
{
    public string Nome { get; private set; }
    public DateTime DataDeCadastro { get; private set; }
    
    // Propriedade de navegação para o relacionamento many-to-many
    public ICollection<Domain.Diario> Diarios { get; private set; }

    public Etiqueta(string nome)
    {
        Nome = nome;
        DataDeCadastro = DateTime.UtcNow;
        Diarios = new List<Domain.Diario>();
    }

    // Construtor para o EF
    protected Etiqueta() { }

    public void AtualizarNome(string novoNome)
    {
        Nome = novoNome;
    }
}
