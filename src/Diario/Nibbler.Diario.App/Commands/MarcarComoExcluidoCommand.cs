using Nibbler.Core.Messages;
using FluentValidation;

namespace Nibbler.Diario.App.Commands;

public class MarcarComoExcluidoCommand : Command
{
    public Guid DiarioId { get; private set; }

    public MarcarComoExcluidoCommand(Guid diarioId)
    {
        DiarioId = diarioId;
    }

    public override bool EstaValido()
    {
        ValidationResult = new MarcarComoExcluidoValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class MarcarComoExcluidoValidation : AbstractValidator<MarcarComoExcluidoCommand>
    {
        public MarcarComoExcluidoValidation()
        {
            RuleFor(c => c.DiarioId)
                .NotEqual(Guid.Empty).WithMessage("Id do diário inválido.");
        }
    }
}