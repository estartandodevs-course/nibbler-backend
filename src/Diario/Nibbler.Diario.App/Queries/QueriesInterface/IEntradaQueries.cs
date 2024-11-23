using Nibbler.Diario.App.ViewModels;

public interface IEntradaQueries
{
    Task<IEnumerable<EntradaViewModel>> ObterPorDiario(Guid diarioId);
    Task<EntradaViewModel> ObterPorId(Guid id);
    Task<IEnumerable<EntradaViewModel>> ObterPorPeriodo(Guid diarioId, DateTime inicio, DateTime fim);
}