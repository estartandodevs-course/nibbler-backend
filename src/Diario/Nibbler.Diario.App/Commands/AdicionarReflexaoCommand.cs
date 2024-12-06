using Nibbler.Core.Messages;
using FluentValidation;

namespace Nibbler.Diario.App.Commands;

public class AdicionarReflexaoCommand : Command
{
    public Guid UsuarioId { get; private set; }
    public string Conteudo { get; private set; }
    public Guid? EmocaoId { get; private set; }

    public AdicionarReflexaoCommand(Guid usuarioId, string conteudo, Guid? emocaoId = null)
    {
        UsuarioId = usuarioId;
        Conteudo = conteudo;
        EmocaoId = emocaoId;
    }

    public override bool EstaValido()
    {
        ValidationResult = new AdicionarReflexaoValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class AdicionarReflexaoValidation : AbstractValidator<AdicionarReflexaoCommand>
    {
        public AdicionarReflexaoValidation()
        {
            RuleFor(c => c.UsuarioId)
                .NotEqual(Guid.Empty).WithMessage("Id do usuário inválido.");

            RuleFor(c => c.Conteudo)
                .NotEmpty().WithMessage("O conteúdo da reflexão é obrigatório.");
        }
    }
}