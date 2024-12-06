using FluentValidation;
using Nibbler.Core.Messages;

namespace Nibbler.Diario.app.Commands;

public class AssociarEmocaoNaReflexaoCommand : Command
{
    public Guid ReflexaoId { get; private set; }
    public Guid EmocaoId { get; private set; }

    public AssociarEmocaoNaReflexaoCommand(Guid reflexaoId, Guid emocaoId)
    {
        ReflexaoId = reflexaoId;
        EmocaoId = emocaoId;
    }

    public override bool EstaValido()
    {
        ValidationResult = new AssociarEmocaoNaReflexaoValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class AssociarEmocaoNaReflexaoValidation : AbstractValidator<AssociarEmocaoNaReflexaoCommand>
    {
        public AssociarEmocaoNaReflexaoValidation()
        {
            RuleFor(c => c.ReflexaoId)
                .NotEqual(Guid.Empty).WithMessage("Id da reflexão inválido.");

            RuleFor(c => c.EmocaoId)
                .NotEqual(Guid.Empty).WithMessage("Id da emoção inválido.");
        }
    }
}