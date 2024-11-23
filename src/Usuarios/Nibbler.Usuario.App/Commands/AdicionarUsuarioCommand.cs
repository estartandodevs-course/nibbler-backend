using FluentValidation;
using Nibbler.Core.Messages;

namespace Nibbler.Usuario.App.Commands;

public class AdicionarUsuarioCommand : Command
{
    public string Nome { get; private set; }
    public DateTime DataDeNascimento { get; private set; }
    public string Foto { get; private set; }
    public string Email { get; private set; }
    public string Senha { get; private set; }

    public AdicionarUsuarioCommand(string nome, DateTime dataDeNascimento, string foto, string email, string senha)
    {
        Nome = nome;
        DataDeNascimento = dataDeNascimento;
        Foto = foto;
        Email = email;
        Senha = senha;
    }

    public override bool EstaValido()
    {
        ValidationResult = new AdicionarUsuarioValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class AdicionarUsuarioValidation : AbstractValidator<AdicionarUsuarioCommand>
    {
        public AdicionarUsuarioValidation()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório")
                .Length(2, 200).WithMessage("O nome deve ter entre 2 e 200 caracteres");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("O email é obrigatório")
                .EmailAddress().WithMessage("Email inválido");

            RuleFor(c => c.Senha)
                .NotEmpty().WithMessage("A senha é obrigatória")
                .Length(6, 100).WithMessage("A senha deve ter entre 6 e 100 caracteres");
        }
    }
}
