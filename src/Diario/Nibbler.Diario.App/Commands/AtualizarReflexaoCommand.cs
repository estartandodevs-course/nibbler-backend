using FluentValidation;
using Nibbler.Core.Messages;

namespace Nibbler.Diario.app.Commands;

public class AtualizarReflexaoCommand : Command
{
    public Guid Id { get; private set; }
    public string Conteudo { get; private set; }
    public Guid? EmocaoId { get; private set; }

    public AtualizarReflexaoCommand(Guid id, string conteudo, Guid? emocaoId = null)
    {
        Id = id;
        Conteudo = conteudo;
        EmocaoId = emocaoId;
    }

    public override bool EstaValido()
    {
        ValidationResult = new AtualizarReflexaoValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class AtualizarReflexaoValidation : AbstractValidator<AtualizarReflexaoCommand>
    {
        public AtualizarReflexaoValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty).WithMessage("Id da reflexão inválido.");

            RuleFor(c => c.Conteudo)
                .NotEmpty().WithMessage("O conteúdo da reflexão é obrigatório.");
        }
    }
}