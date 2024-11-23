using Nibbler.Core.DomainObjects;

namespace Nibbler.Tarefa.Domain;

public class Tarefa : Entity, IAggregateRoot
{
    public string Titulo { get; private set; }
    public string Descricao { get; private set; }
    public bool Concluida { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Guid CategoriaId { get; private set; }
    
    // Navegação EF
    public Categoria Categoria { get; private set; }
    
    private readonly List<ItemTarefa> _itens;
    public IReadOnlyCollection<ItemTarefa> Itens => _itens;

    protected Tarefa()
    {
        _itens = new List<ItemTarefa>();
    }

    public Tarefa(string titulo, string descricao, Guid usuarioId, Categoria categoria)
    {
        Titulo = titulo;
        Descricao = descricao;
        UsuarioId = usuarioId;
        AtribuirCategoria(categoria);
        Concluida = false;
        _itens = new List<ItemTarefa>();
    }

    public void AtribuirCategoria(Categoria categoria)
    {
        Categoria = categoria;
        CategoriaId = categoria.Id;
        DataDeAlteracao = DateTime.Now;
    }

    public void AdicionarItem(string descricao, Guid usuarioId)
    {
        var ordem = _itens.Count;
        var item = new ItemTarefa(Id, usuarioId, descricao, ordem);
        _itens.Add(item);
        DataDeAlteracao = DateTime.Now;
    }

    public void MarcarComoConcluida()
    {
        Concluida = true;
        DataDeAlteracao = DateTime.Now;
    }

    public void DesmarcarComoConcluida()
    {
        Concluida = false;
        DataDeAlteracao = DateTime.Now;
    }

    public void AlterarTitulo(string titulo)
    {
        Titulo = titulo;
        DataDeAlteracao = DateTime.Now;
    }

    public void AlterarDescricao(string descricao)
    {
        Descricao = descricao;
        DataDeAlteracao = DateTime.Now;
    }
}