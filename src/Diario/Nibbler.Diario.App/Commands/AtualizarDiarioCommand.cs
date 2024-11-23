using Nibbler.Core.Messages;
using FluentValidation;

namespace Nibbler.Diario.App.Commands;

public class AtualizarDiarioCommand : Command
{
    public Guid DiarioId { get; private set; }
    public string Titulo { get; private set; }
    public string Conteudo { get; private set; }

    public AtualizarDiarioCommand(Guid diarioId, string titulo, string conteudo)
    {
        DiarioId = diarioId;
        Titulo = titulo;
        Conteudo = conteudo;
    }

    public override bool EstaValido()
    {
        ValidationResult = new AtualizarDiarioValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class AtualizarDiarioValidation : AbstractValidator<AtualizarDiarioCommand>
    {
        public AtualizarDiarioValidation()
        {
            RuleFor(c => c.DiarioId)
                .NotEqual(Guid.Empty).WithMessage("Id do diário inválido.");

            RuleFor(c => c.Titulo)
                .NotEmpty().WithMessage("O título do diário é obrigatório.");

            RuleFor(c => c.Conteudo)
                .NotEmpty().WithMessage("O conteúdo do diário é obrigatório.");
        }
    }
}