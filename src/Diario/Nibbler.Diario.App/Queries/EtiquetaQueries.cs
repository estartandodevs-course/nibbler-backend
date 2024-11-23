
using Nibbler.Diario.App.ViewModels;
using Nibbler.Diario.Domain;
using Nibbler.Diario.Domain.Interfaces;

namespace Nibbler.Diario.App.Queries;

public class EtiquetaQueries : IEtiquetaQueries
{
    private readonly IDiarioRepository _diarioRepository;

    public EtiquetaQueries(IDiarioRepository diarioRepository)
    {
        _diarioRepository = diarioRepository;
    }

    public async Task<IEnumerable<EtiquetaViewModel>> ObterTodas()
    {
        var etiquetas = await _diarioRepository.ObterTodasEtiquetas();
        return etiquetas.Select(MapearParaViewModel);
    }

    public async Task<EtiquetaViewModel> ObterPorId(Guid id)
    {
        var etiqueta = await _diarioRepository.ObterEtiquetaPorId(id);
        return etiqueta == null ? null : MapearParaViewModel(etiqueta);
    }
    

    private static EtiquetaViewModel MapearParaViewModel(Etiqueta etiqueta)
    {
        return new EtiquetaViewModel
        {
            Id = etiqueta.Id,
            Nome = etiqueta.Nome
        };
    }
}