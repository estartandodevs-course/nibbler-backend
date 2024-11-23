using FluentValidation.Results;
using Nibbler.Core.Messages;

namespace Nibbler.Core.Mediator;

public interface IMediatorHandler
{
    Task PublicarEvento<T>(T evento) where T : Event;

    Task<ValidationResult> EnviarComando<T>(T comando) where T : Command;
}
