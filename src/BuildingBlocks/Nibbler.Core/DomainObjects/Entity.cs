using System.Globalization;
using Nibbler.Core.Messages;

namespace Nibbler.Core.DomainObjects;

public abstract class Entity
{
    public Guid Id { get; set; }

    public DateTime DataDeCadastro { get; set; }

    public DateTime DataDeAlteracao { get; set; }

    private List<Event> _notificacoes;
    public IReadOnlyCollection<Event> Notificacoes => _notificacoes?.AsReadOnly();

    public Entity()
    {
        DataDeCadastro = DateTime.Now;
        DataDeAlteracao = DateTime.Now;
    }

    public string ObterDataFormatada() => DataDeCadastro.ToString("G", new CultureInfo("pt-Br"));

    public void AtribuirEntidadeId(Guid id) => Id = id;

    public void AdicionarEvento(Event evento)
    {
        _notificacoes ??= new List<Event>();
        _notificacoes.Add(evento);
    }

    public void RemoverEvento(Event eventItem)
    {
        _notificacoes?.Remove(eventItem);
    }

    public void LimparEventos()
    {
        _notificacoes?.Clear();
    }
}