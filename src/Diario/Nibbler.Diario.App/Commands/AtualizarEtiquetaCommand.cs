using Nibbler.Core.Messages;
using FluentValidation;

namespace Nibbler.Diario.App.Commands;

public class AtualizarEtiquetaCommand : Command
{
    public Guid EtiquetaId { get; private set; }
    public string Nome { get; private set; }

    public AtualizarEtiquetaCommand(Guid etiquetaId, string nome)
    {
        EtiquetaId = etiquetaId;
        Nome = nome;
    }

    public override bool EstaValido()
    {
        ValidationResult = new AtualizarEtiquetaValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class AtualizarEtiquetaValidation : AbstractValidator<AtualizarEtiquetaCommand>
    {
        public AtualizarEtiquetaValidation()
        {
            RuleFor(c => c.EtiquetaId)
                .NotEqual(Guid.Empty).WithMessage("O ID da etiqueta é inválido.");

            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome da etiqueta é obrigatório.")
                .Length(2, 50).WithMessage("O nome da etiqueta deve ter entre 2 e 50 caracteres.");
        }
    }
}