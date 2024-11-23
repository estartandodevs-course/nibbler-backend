using FluentValidation;
using Nibbler.Core.Messages;

namespace Nibbler.Diario.App.Commands;

public class AdicionarEtiquetaAoDiarioCommand : Command
{
    public Guid DiarioId { get; private set; }
    public Guid EtiquetaId { get; private set; }

    public AdicionarEtiquetaAoDiarioCommand(Guid diarioId, Guid etiquetaId)
    {
        DiarioId = diarioId;
        EtiquetaId = etiquetaId;
    }

    public override bool EstaValido()
    {
        ValidationResult = new AdicionarEtiquetaAoDiarioValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class AdicionarEtiquetaAoDiarioValidation : AbstractValidator<AdicionarEtiquetaAoDiarioCommand>
    {
        public AdicionarEtiquetaAoDiarioValidation()
        {
            RuleFor(c => c.DiarioId)
                .NotEqual(Guid.Empty).WithMessage("O ID do diário é inválido");

            RuleFor(c => c.EtiquetaId)
                .NotEqual(Guid.Empty).WithMessage("O ID da etiqueta é inválido");
        }
    }
} 