using FluentValidation;
using Nibbler.Core.Messages;

public class RemoverEmocaoDaReflexaoCommand : Command
{
    public Guid ReflexaoId { get; private set; }

    public RemoverEmocaoDaReflexaoCommand(Guid reflexaoId)
    {
        ReflexaoId = reflexaoId;
    }

    public override bool EstaValido()
    {
        ValidationResult = new RemoverEmocaoDaReflexaoValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class RemoverEmocaoDaReflexaoValidation : AbstractValidator<RemoverEmocaoDaReflexaoCommand>
    {
        public RemoverEmocaoDaReflexaoValidation()
        {
            RuleFor(c => c.ReflexaoId)
                .NotEqual(Guid.Empty).WithMessage("Id da reflexão inválido.");
        }
    }
}