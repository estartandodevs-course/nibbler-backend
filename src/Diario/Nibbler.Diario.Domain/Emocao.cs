using Nibbler.Core.DomainObjects;

namespace Nibbler.Diario.Domain;

public class Emocao : Entity
{
    public string Nome { get; private set; }
    public DateTime DataRegistro { get; private set; }
    
    public virtual ICollection<Reflexao> Reflexoes { get; private set; }

    protected Emocao() 
    {
        Reflexoes = new List<Reflexao>();
    }

    public Emocao(string nome) : this()
    {
        Nome = nome;
        DataRegistro = DateTime.UtcNow;
    }

    public void AtualizarNome(string nome)
    {
        Nome = nome;
    }
}