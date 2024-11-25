using Nibbler.Core.DomainObjects;
using Nibbler.Diario.Domain;

public class Etiqueta : Entity
{
    public string Nome { get; private set; }
    public DateTime DataDeCadastro { get; private set; }
    
    public ICollection<Diario> Diarios { get; private set; }

    public Etiqueta(string nome, string descricao = null)
    {
        Nome = nome;
        DataDeCadastro = DateTime.UtcNow;
        Diarios = new List<Diario>();
    }

    // Construtor para o EF
    protected Etiqueta() { }

    public void AtualizarNome(string novoNome)
    {
        Nome = novoNome;
    }
}
