using Nibbler.Diario.App.ViewModels;

namespace Nibbler.Diario.App.Queries;

public interface IEtiquetaQueries
{
    Task<IEnumerable<EtiquetaViewModel>> ObterTodas();
    Task<EtiquetaViewModel> ObterPorId(Guid id);
}