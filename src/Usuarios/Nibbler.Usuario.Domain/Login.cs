using Nibbler.Core.DomainObjects;
using Nibbler.Core.ValueObjects;

namespace Nibbler.Usuario.Domain;

public sealed class Login
{
    public Guid Hash { get; private set; }
    public Email Email { get; private set; }
    public Senha Senha { get; private set; }

    protected Login(){}

    public Login(Email email, Senha senha)
    {
        Email = email;
        Senha = senha;
        Hash = new Identidade(Email.Endereco,Senha.Valor);
    }
}