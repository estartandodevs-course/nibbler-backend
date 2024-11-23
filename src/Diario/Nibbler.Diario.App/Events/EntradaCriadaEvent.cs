using Nibbler.Core.Messages;

namespace Nibbler.Diario.Domain.Events;

public class EntradaCriadaEvent : Event
{
    public Guid DiarioId { get; private set; }
    public Guid EntradaId { get; private set; }
    public DateTime DataDaEntrada { get; private set; }

    public EntradaCriadaEvent(Guid diarioId, Guid entradaId, DateTime dataDaEntrada)
    {
        DiarioId = diarioId;
        EntradaId = entradaId;
        DataDaEntrada = dataDaEntrada;
        AggregateId = diarioId;
    }
}
