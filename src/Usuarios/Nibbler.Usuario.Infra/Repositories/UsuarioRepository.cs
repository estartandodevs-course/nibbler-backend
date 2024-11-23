using Microsoft.EntityFrameworkCore;
using Nibbler.Core.Data;
using Nibbler.Usuario.Domain.Interfaces;
using Nibbler.Usuario.Infra.Data;

namespace Nibbler.Usuario.Infra.Repositories;


public class UsuarioRepository : IUsuarioRepository
{
    private readonly UsuarioContext _context;

    public UsuarioRepository(UsuarioContext context)
    {
        _context = context;
    }

    public IUnitOfWorks UnitOfWork => _context;

    public void Adicionar(Domain.Usuario entity)
    {
        _context.Usuarios.Add(entity);
    }

    public void Apagar(Func<Domain.Usuario, bool> predicate)
    {
        var usuarios = _context.Usuarios.Where(predicate).ToList();

        _context.Usuarios.RemoveRange(usuarios);
    }

    public void Atualizar(Domain.Usuario entity)
    {
        _context.Usuarios.Update(entity);
    }

    public async Task<Domain.Usuario> ObterPorId(Guid Id)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(c => c.Id == Id);
    }
    
    public async Task<IEnumerable<Domain.Usuario>> ObterTodos()
    {
        return await _context.Usuarios.OrderBy(c => c.Nome).ToListAsync();
    }
    
    public void Dispose()
    {
        _context?.Dispose();
    }
}
