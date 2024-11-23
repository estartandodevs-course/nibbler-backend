using FluentValidation;
using Nibbler.Core.Messages;

namespace Nibbler.Diario.App.Commands;

public class AdicionarEtiquetaCommand : Command
{
    public Guid DiarioId { get; private set; }
    public string Nome { get; private set; }

    public AdicionarEtiquetaCommand(Guid diarioId, string nome)
    {
        DiarioId = diarioId;
        Nome = nome;
    }

    public override bool EstaValido()
    {
        ValidationResult = new AdicionarEtiquetaValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class AdicionarEtiquetaValidation : AbstractValidator<AdicionarEtiquetaCommand>
    {
        public AdicionarEtiquetaValidation()
        {
            RuleFor(c => c.DiarioId)
                .NotEqual(Guid.Empty).WithMessage("O ID do diário é inválido");

            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome da etiqueta é obrigatório")
                .Length(2, 50).WithMessage("O nome deve ter entre 2 e 50 caracteres");
        }
    }
}