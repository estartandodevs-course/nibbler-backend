using Nibbler.Diario.App.ViewModels;

namespace Nibbler.Diario.app.Queries.QueriesInterface;
public interface IReflexaoQueries
{
    Task<ReflexaoViewModel> ObterPorId(Guid id);
    Task<IEnumerable<ReflexaoViewModel>> ObterPorUsuario(Guid usuarioId);
    Task<IEnumerable<ReflexaoViewModel>> ObterTodas();
}