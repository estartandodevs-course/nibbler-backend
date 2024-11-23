using FluentValidation;
using Nibbler.Core.Messages;

namespace Nibbler.Diario.App.Commands;

public class CriarEtiquetaCommand : Command
{
    public string Nome { get; private set; }

    public CriarEtiquetaCommand(string nome)
    {
        Nome = nome;
    }

    public override bool EstaValido()
    {
        ValidationResult = new CriarEtiquetaValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class CriarEtiquetaValidation : AbstractValidator<CriarEtiquetaCommand>
    {
        public CriarEtiquetaValidation()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome da etiqueta é obrigatório")
                .Length(2, 50).WithMessage("O nome deve ter entre 2 e 50 caracteres");
        }
    }
} 