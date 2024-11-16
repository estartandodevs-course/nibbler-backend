using System.Text.RegularExpressions;

namespace Nibbler.Core.ValueObjects;

public class Email
{
    public const int EmailMaximo = 255;

    public string Endereco { get; private set; }

    protected Email() { }

    public Email(string endereco)
    {
        Endereco = endereco;
    }

    public bool EstaValido()
    {
        if (string.IsNullOrWhiteSpace(Endereco)) return false;

        var regex = new Regex(@"[\w\.-]+(\+[\w-]*)?@([\w-]+\.)+[\w-]+");

        var match = regex.Match(Endereco);

        return match.Success;
    }

    public override bool Equals(object email)
    {
        var Email = (Email)email;

        return Endereco.Trim().ToLower() == Email.Endereco.Trim().ToLower();
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return Endereco;
    }
}