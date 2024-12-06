using FluentValidation;
using Nibbler.Core.Messages;

namespace Nibbler.Diario.app.Commands;

public class ExcluirReflexaoCommand : Command
{
    public Guid Id { get; private set; }

    public ExcluirReflexaoCommand(Guid id)
    {
        Id = id;
    }

    public override bool EstaValido()
    {
        ValidationResult = new ExcluirReflexaoValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class ExcluirReflexaoValidation : AbstractValidator<ExcluirReflexaoCommand>
    {
        public ExcluirReflexaoValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty).WithMessage("Id da reflexão inválido.");
        }
    }
}