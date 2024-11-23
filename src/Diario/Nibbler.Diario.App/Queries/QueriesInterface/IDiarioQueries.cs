using Nibbler.Diario.App.ViewModels;

namespace Nibbler.Diario.app.Queries.QueriesInterface;

public interface IDiarioQueries
{
    Task<DiarioViewModel> ObterPorId(Guid id);
    Task<IEnumerable<DiarioViewModel>> ObterPorUsuario(Guid usuarioId);
    Task<IEnumerable<DiarioViewModel>> ObterPorEtiqueta(string etiqueta);
    Task<IEnumerable<DiarioViewModel>> ObterTodosAtivos();
    Task<IEnumerable<DiarioViewModel>> ObterTodosInativos();
    Task<IEnumerable<EntradaViewModel>> ObterEntradasPorDiario(Guid diarioId);
    Task<EntradaViewModel> ObterEntradaPorId(Guid diarioId, Guid entradaId);
}