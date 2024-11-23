using System.Security.Cryptography;
using System.Text;

namespace Nibbler.Core.DomainObjects;

public readonly struct Identidade
{
    private readonly Guid _id;

    public Identidade(params object[] keys)
    {
        var hash = MD5.HashData(Encoding.Default.GetBytes(string.Concat(keys)));
        _id = new Guid(hash);
    }

    public static implicit operator Guid(Identidade identidade) => identidade._id;
}
