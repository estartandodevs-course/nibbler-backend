using Nibbler.Core.Data;

namespace Nibbler.Usuario.Domain.Interfaces;

public interface IUsuarioRepository : IRepository<Usuario>, IDisposable
{
    Task<IEnumerable<Usuario>> ObterTodos();
}
