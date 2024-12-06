using Nibbler.Diario.app.Queries.QueriesInterface;
using Nibbler.Diario.App.ViewModels;
using Nibbler.Diario.Domain.Interfaces;

namespace Nibbler.Diario.App.Queries;

public class EmocaoQueries : IEmocaoQueries
{
    private readonly IDiarioRepository _diarioRepository;

    public EmocaoQueries(IDiarioRepository diarioRepository)
    {
        _diarioRepository = diarioRepository;
    }

    public async Task<EmocaoViewModel> ObterPorId(Guid id)
    {
        var emocao = await _diarioRepository.ObterEmocaoPorId(id);
        return emocao == null ? null : EmocaoViewModel.Mapear(emocao);
    }

    public async Task<IEnumerable<EmocaoViewModel>> ObterTodas()
    {
        var emocoes = await _diarioRepository.ObterTodasEmocoes();
        return emocoes.Select(EmocaoViewModel.Mapear);
    }
}