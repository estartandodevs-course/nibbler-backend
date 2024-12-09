using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Nibbler.Core.Data;
using Nibbler.Diario.Domain;
using Nibbler.Diario.Domain.Interfaces;
using Nibbler.Diario.Infra.Data;

namespace Nibbler.Diario.Infra.Repositories;

public class DiarioRepository : IDiarioRepository
{
    private readonly DiarioContext _context;

    public DiarioRepository(DiarioContext context)
    {
        _context = context;
    }

    public IUnitOfWorks UnitOfWork => _context;

    public void Adicionar(Domain.Diario diario)
    {
        _context.Diarios.Add(diario);
    }
    public void AtualizarU(Domain.Usuario usuario)
    {
        _context.Diarios
            .Where(c => c.Usuario.Id == usuario.Id)
            .ExecuteUpdate(set => set
                .SetProperty(c => c.Usuario.Nome, usuario.Nome)
                .SetProperty(c => c.Usuario.CaminhoFoto, usuario.CaminhoFoto));
    }

    public void Adicionar(Reflexao reflexao)
    {
        _context.Reflexoes.Add(reflexao);
    }

    public void Atualizar(Reflexao reflexao)
    {
        _context.Reflexoes.Update(reflexao);
    }

    public void Excluir(Reflexao reflexao)
    {
        _context.Reflexoes.Remove(reflexao);
    }
    
    public void Adicionar(Emocao emocao)
    {
        _context.Emocoes.Add(emocao);
    }

    public void Atualizar(Emocao emocao)
    {
        _context.Emocoes.Update(emocao);
    }

    public void Excluir(Emocao emocao)
    {
        _context.Emocoes.Remove(emocao);
    }

    public async Task<Reflexao> ObterReflexaoPorId(Guid id)
    {
        return await _context.Reflexoes
            .Include(r => r.Emocao)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Reflexao>> ObterReflexoesPorUsuario(Guid usuarioId)
    {
        return await _context.Reflexoes
            .Include(r => r.Emocao)
            .Where(r => r.UsuarioId == usuarioId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reflexao>> ObterTodasReflexoes()
    {
        return await _context.Reflexoes
            .Include(r => r.Emocao)
            .ToListAsync();
    }

    public async Task<Emocao> ObterEmocaoPorId(Guid id)
    {
        return await _context.Emocoes
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Emocao>> ObterTodasEmocoes()
    {
        return await _context.Emocoes.ToListAsync();
    }

    public async Task<bool> ExisteEmocao(Guid id)
    {
        return await _context.Emocoes.AnyAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Etiqueta>> ObterEtiquetasPorDiario(Guid diarioId)
    {
        var diario = await _context.Diarios
            .Include(d => d.Etiquetas)
            .FirstOrDefaultAsync(d => d.Id == diarioId);

        return diario?.Etiquetas ?? new List<Etiqueta>();
    }

    public void Adicionar(Etiqueta etiqueta)
    {
        _context.Etiquetas.Add(etiqueta);
    }

    public void Atualizar(Domain.Diario diario)
    {
        _context.Diarios.Update(diario);
    }

    public void Atualizar(Etiqueta etiqueta)
    {
        _context.Etiquetas.Update(etiqueta);
    }
    

    public void Atualizar(Domain.Usuario usuario)
    {
        _context.Diarios
            .Where(c => c.Usuario.Id == usuario.Id)
            .ExecuteUpdate(set => set
                .SetProperty(c => c.Usuario.Nome, usuario.Nome)
                .SetProperty(c => c.Usuario.CaminhoFoto, usuario.CaminhoFoto));
    }

    public async Task ExcluirEtiqueta(Guid id)
    {
        var etiqueta = await _context.Etiquetas
            .Include(e => e.Diarios)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (etiqueta != null)
        {
            // Limpa os relacionamentos
            etiqueta.Diarios.Clear();
        
            // Remove a etiqueta
            _context.Etiquetas.Remove(etiqueta);
        }
    }

    public void Apagar(Func<Domain.Diario, bool> predicate)
    {
        var diarios = _context.Diarios.Where(predicate).ToList();
        _context.Diarios.RemoveRange(diarios);
    }

    public async Task<Domain.Diario> ObterPorId(Guid id)
    {
        return await _context.Diarios
            .Include(d => d.Usuario)
            .Include(d => d.Etiquetas)
            .Include(d => d.Reflexoes)
            .FirstOrDefaultAsync(d => d.Id == id && d.DataDeExclusao == null);
    }

    public async Task<Domain.Diario> ObterDiarioPorId(Guid id)
    {
        return await ObterPorId(id);
    }

    public async Task<IEnumerable<Domain.Diario>> ObterPorUsuario(Guid usuarioId)
    {
        return await _context.Diarios
            .Include(d => d.Usuario)
            .Include(d => d.Etiquetas)
            .Include(d => d.Reflexoes)
            .Where(d => d.Usuario.Id == usuarioId && d.DataDeExclusao == null)
            .OrderByDescending(d => d.DataDeCadastro)
            .ToListAsync();
    }

    public async Task<IEnumerable<Domain.Diario>> ObterTodosAtivos()
    {
        return await _context.Diarios
            .Include(d => d.Usuario)
            .Include(d => d.Etiquetas)
            .Include(d => d.Reflexoes)
            .Where(d => d.DataDeExclusao == null)
            .OrderByDescending(d => d.DataDeCadastro)
            .ToListAsync();
    }

    public async Task<IEnumerable<Domain.Diario>> ObterPorEtiqueta(string etiqueta)
    {
        return await _context.Diarios
            .Include(d => d.Usuario)
            .Include(d => d.Etiquetas)
            .Include(d => d.Reflexoes)
            .Where(d => 
                d.DataDeExclusao == null && 
                d.Etiquetas.Any(e => e.Nome.ToLower() == etiqueta.ToLower()))
            .OrderByDescending(d => d.DataDeCadastro)
            .ToListAsync();
    }

    public async Task<IEnumerable<Etiqueta>> ObterTodasEtiquetas()
    {
        return await _context.Etiquetas
            .OrderBy(e => e.Nome)
            .ToListAsync();
    }

    public async Task<Etiqueta> ObterEtiquetaPorId(Guid id)
    {
        return await _context.Etiquetas
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task MarcarComoExcluidoAsync(Guid diarioId)
    {
        var diario = await ObterPorId(diarioId);
        if (diario != null)
        {
            diario.MarcarComoExcluido();
            Atualizar(diario);
        }
    }
    public async Task<Domain.Usuario> ObterUsuarioDiarioPorId(Guid id)
    {
        return await _context.Set<Domain.Usuario>()
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    public async Task<IEnumerable<Domain.Diario>> ObterTodosInativos()
    {
        return await _context.Diarios
            .Include(d => d.Usuario)
            .Include(d => d.Etiquetas)
            .Include(d => d.Reflexoes)
            .Where(d => d.DataDeExclusao != null)
            .OrderByDescending(d => d.DataDeCadastro)
            .ToListAsync();
    }

    public async Task<IEnumerable<Entrada>> ObterEntradasPorDiario(Guid diarioId)
    {
        return await _context.Entradas
            .Where(e => e.DiarioId == diarioId)
            .OrderByDescending(e => e.DataDeCadastro)
            .ToListAsync();
    }

    public async Task<Entrada> ObterEntradaPorId(Guid diarioId, Guid entradaId)
    {
        return await _context.Entradas
            .FirstOrDefaultAsync(e => e.DiarioId == diarioId && e.Id == entradaId);
    }

    public async Task<IEnumerable<Domain.Diario>> ObterPorEtiquetaAsync(string etiqueta)
    {
        return await _context.Diarios
            .Include(d => d.Usuario)
            .Include(d => d.Etiquetas)
            .Include(d => d.Reflexoes)
            .Where(d => 
                d.DataDeExclusao == null && 
                d.Etiquetas.Any(e => e.Nome.ToLower() == etiqueta.ToLower()))
            .OrderByDescending(d => d.DataDeCadastro)
            .ToListAsync();
    }

    public DbConnection ObterConexao()
    {
        return _context.Database.GetDbConnection();
    }
    
    public void Dispose()
    {
        _context?.Dispose();
    }
}