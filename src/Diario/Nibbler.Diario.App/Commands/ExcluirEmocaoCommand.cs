using FluentValidation;
using Nibbler.Core.Messages;

namespace Nibbler.Diario.app.Commands;

public class ExcluirEmocaoCommand : Command
{
    public Guid Id { get; private set; }

    public ExcluirEmocaoCommand(Guid id)
    {
        Id = id;
    }

    public override bool EstaValido()
    {
        ValidationResult = new ExcluirEmocaoValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class ExcluirEmocaoValidation : AbstractValidator<ExcluirEmocaoCommand>
    {
        public ExcluirEmocaoValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty).WithMessage("Id da emoção inválido.");
        }
    }
}