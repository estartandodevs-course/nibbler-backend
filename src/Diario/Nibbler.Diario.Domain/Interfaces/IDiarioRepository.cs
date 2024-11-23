using System.Data.Common;
using Nibbler.Core.Data;

namespace Nibbler.Diario.Domain.Interfaces;

public interface IDiarioRepository : IDisposable
{
    IUnitOfWorks UnitOfWork { get; }
    DbConnection ObterConexao();

    // Métodos de Diário
    void Adicionar(Domain.Diario diario);
    void Atualizar(Domain.Diario diario);
    void Apagar(Func<Domain.Diario, bool> predicate);
    Task<Domain.Diario> ObterPorId(Guid id);
    Task<IEnumerable<Domain.Diario>> ObterPorUsuario(Guid usuarioId);
    Task<IEnumerable<Domain.Diario>> ObterTodosAtivos();
    Task<IEnumerable<Domain.Diario>> ObterTodosInativos();
    Task<IEnumerable<Domain.Diario>> ObterPorEtiqueta(string etiqueta);
    Task MarcarComoExcluidoAsync(Guid diarioId);

    // Métodos de Etiqueta
    void Adicionar(Etiqueta etiqueta);
    void Atualizar(Etiqueta etiqueta);
    Task ExcluirEtiqueta(Guid id);
    Task<Etiqueta> ObterEtiquetaPorId(Guid id);
    Task<IEnumerable<Etiqueta>> ObterTodasEtiquetas();
    Task<IEnumerable<Etiqueta>> ObterEtiquetasPorDiario(Guid diarioId);

    // Métodos de Entrada
    Task<IEnumerable<Entrada>> ObterEntradasPorDiario(Guid diarioId);
    Task<Entrada> ObterEntradaPorId(Guid diarioId, Guid entradaId);
    

    // Métodos de Usuário
    Task<Domain.Usuario> ObterUsuarioDiarioPorId(Guid id);
}