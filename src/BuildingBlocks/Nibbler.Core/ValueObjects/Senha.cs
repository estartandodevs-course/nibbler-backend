using Nibbler.Core.DomainObjects;

namespace Nibbler.Core.ValueObjects;

public class Senha : Aggregate
{
    public string Valor { get; private set; }

    protected Senha() { }

    public Senha(string valor)
    {
        Valor = ObterHash(valor);
    }
}
