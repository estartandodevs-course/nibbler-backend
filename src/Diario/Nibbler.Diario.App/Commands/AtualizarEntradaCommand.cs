using FluentValidation;
using Nibbler.Core.Messages;

namespace Nibbler.Diario.App.Commands;

public class AtualizarEntradaCommand : Command
{
    public Guid DiarioId { get; private set; }
    public Guid EntradaId { get; private set; }
    public string Conteudo { get; private set; }

    public AtualizarEntradaCommand(Guid diarioId, Guid entradaId, string conteudo)
    {
        DiarioId = diarioId;
        EntradaId = entradaId;
        Conteudo = conteudo;
    }

    public override bool EstaValido()
    {
        ValidationResult = new AtualizarEntradaValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class AtualizarEntradaValidation : AbstractValidator<AtualizarEntradaCommand>
    {
        public AtualizarEntradaValidation()
        {
            RuleFor(c => c.DiarioId)
                .NotEqual(Guid.Empty).WithMessage("Id do diário inválido");

            RuleFor(c => c.EntradaId)
                .NotEqual(Guid.Empty).WithMessage("Id da entrada inválido");

            RuleFor(c => c.Conteudo)
                .NotEmpty().WithMessage("O conteúdo da entrada é obrigatório")
                .MaximumLength(5000).WithMessage("O conteúdo pode ter no máximo 5000 caracteres");
        }
    }
}