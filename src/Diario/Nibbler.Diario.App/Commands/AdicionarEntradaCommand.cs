using FluentValidation;
using Nibbler.Core.Messages;

namespace Nibbler.Diario.App.Commands;

public class AdicionarEntradaCommand : Command
{
    public Guid DiarioId { get; private set; }
    public string Conteudo { get; private set; }

    public AdicionarEntradaCommand(Guid diarioId, string conteudo)
    {
        DiarioId = diarioId;
        Conteudo = conteudo;
    }

    public override bool EstaValido()
    {
        ValidationResult = new AdicionarEntradaValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class AdicionarEntradaValidation : AbstractValidator<AdicionarEntradaCommand>
    {
        public AdicionarEntradaValidation()
        {
            RuleFor(c => c.DiarioId)
                .NotEqual(Guid.Empty).WithMessage("Id do diário inválido");

            RuleFor(c => c.Conteudo)
                .NotEmpty().WithMessage("O conteúdo da entrada é obrigatório")
                .MaximumLength(5000).WithMessage("O conteúdo pode ter no máximo 5000 caracteres");
        }
    }
}