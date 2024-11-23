using Nibbler.Core.DomainObjects;

namespace Nibbler.Diario.Domain;
public class Diario : Entity, IAggregateRoot
{
    public const int TamanhoMaximo = 1000;

    public Usuario Usuario { get; private set; }

    public string Titulo { get; private set; }

    public string Conteudo { get; private set; }

    public DateTime? DataDeExclusao { get; private set; } // Marca a data de exclusão, se for excluidp.

    public int QuantidadeReflexoes { get; private set; }

    //EF
    private List<Etiqueta> _Etiquetas;
    public IReadOnlyCollection<Etiqueta> Etiquetas => _Etiquetas;

    private List<Reflexao> _Reflexoes;
    public IReadOnlyCollection<Reflexao> Reflexoes => _Reflexoes;

    private readonly List<Entrada> _Entradas = new();
    public IReadOnlyCollection<Entrada> Entradas => _Entradas;

    protected Diario()
    {
        _Etiquetas = new List<Etiqueta>();
        _Reflexoes = new List<Reflexao>();
        _Entradas = new List<Entrada>();
    }

    public Diario(Usuario usuario, string titulo, string conteudo) : this()
    {
        Usuario = usuario;
        Titulo = titulo;
        Conteudo = conteudo;
    }

    public void Adicionar(Reflexao reflexao)
    {
        _Reflexoes.Add(reflexao);
        QuantidadeReflexoes++;
    }

    public void Remover(Reflexao reflexao)
    {
        _Reflexoes.Remove(reflexao);
        QuantidadeReflexoes--;
    }

    public void Adicionar(Etiqueta etiqueta)
    {
        _Etiquetas.Add(etiqueta);
    }

    public void Remover(Etiqueta etiqueta)
    {
        _Etiquetas.Remove(etiqueta);
    }
    
    public void MarcarComoExcluido()
    {
        DataDeExclusao = DateTime.UtcNow;
    }

    // Verifica se foi excluído (soft delete)
    public bool FoiExcluido() => DataDeExclusao.HasValue;

    public void AdicionarEntrada(Entrada entrada)
    {
        _Entradas.Add(entrada);
    }

    public void RemoverEntrada(Entrada entrada)
    {
        _Entradas.Remove(entrada);
    }

    public void AdicionarEtiqueta(Etiqueta etiqueta)
    {
        _Etiquetas.Add(etiqueta);
    }
}