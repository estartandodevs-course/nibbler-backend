using Nibbler.Core.DomainObjects;
using Nibbler.Core.Utilities;

namespace Nibbler.Tarefa.Domain;

public class Categoria : Entity
{
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public Guid Hash { get; private set; }
    
    private List<Tarefa> _tarefas;
    
    public IReadOnlyCollection<Tarefa> Tarefas => _tarefas;

    protected Categoria() => 
        _tarefas = new List<Tarefa>(); 
    
    public Categoria(string nome, string descricao, Guid hash)
    {
        Nome = nome;
        Descricao = descricao;
        Hash = new Identidade(Nome.Trim().RemoveAcentos().ToLower());
    }

    public static Guid GerarHash(string nome) => new Identidade(nome.Trim().RemoveAcentos().ToLower());
    
    public void AtribuirNome(string nome)
    {
        Nome = nome;
        Hash = new Identidade(Nome.Trim().RemoveAcentos().ToLower());
    }
    
    public void AtribuirDescricao(string descricao) => Descricao = descricao;
}