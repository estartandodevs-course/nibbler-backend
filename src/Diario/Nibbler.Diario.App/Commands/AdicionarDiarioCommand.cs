using Nibbler.Core.Messages;
using FluentValidation;

namespace Nibbler.Diario.App.Commands;

public class AdicionarDiarioCommand : Command
{
    public Guid UsuarioId { get; private set; }
    public string Titulo { get; private set; }
    public string Conteudo { get; private set; }

    public AdicionarDiarioCommand(Guid usuarioId, string titulo, string conteudo)
    {
        UsuarioId = usuarioId;
        Titulo = titulo;
        Conteudo = conteudo;
    }

    public override bool EstaValido()
    {
        ValidationResult = new AdicionarDiarioValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class AdicionarDiarioValidation : AbstractValidator<AdicionarDiarioCommand>
    {
        public AdicionarDiarioValidation()
        {
            RuleFor(c => c.Titulo)
                .NotEmpty().WithMessage("O título do diário é obrigatório.");

            RuleFor(c => c.Conteudo)
                .NotEmpty().WithMessage("O conteúdo do diário é obrigatório.");
        }
    }
}