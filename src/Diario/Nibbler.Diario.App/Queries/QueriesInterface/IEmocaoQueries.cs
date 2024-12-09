using Nibbler.Diario.App.ViewModels;

namespace Nibbler.Diario.app.Queries.QueriesInterface;
public interface IEmocaoQueries
{
    Task<EmocaoViewModel> ObterPorId(Guid id);
    Task<IEnumerable<EmocaoViewModel>> ObterTodas();
}