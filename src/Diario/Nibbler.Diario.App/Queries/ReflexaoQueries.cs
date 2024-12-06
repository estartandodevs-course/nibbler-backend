using System.Globalization;
using Nibbler.Diario.app.Queries.QueriesInterface;
using Nibbler.Diario.App.ViewModels;
using Nibbler.Diario.Domain;
using Nibbler.Diario.Domain.Interfaces;

namespace Nibbler.Diario.app.Queries;

public class ReflexaoQueries : IReflexaoQueries
{
    private readonly IDiarioRepository _diarioRepository;

    public ReflexaoQueries(IDiarioRepository diarioRepository)
    {
        _diarioRepository = diarioRepository;
    }

    public async Task<ReflexaoViewModel> ObterPorId(Guid id)
    {
        var reflexao = await _diarioRepository.ObterReflexaoPorId(id);
        return reflexao == null ? null : MapearParaViewModel(reflexao);
    }

    public async Task<IEnumerable<ReflexaoViewModel>> ObterPorUsuario(Guid usuarioId)
    {
        var reflexoes = await _diarioRepository.ObterReflexoesPorUsuario(usuarioId);
        return reflexoes.Select(MapearParaViewModel);
    }

    public async Task<IEnumerable<ReflexaoViewModel>> ObterTodas()
    {
        var reflexoes = await _diarioRepository.ObterTodasReflexoes();
        return reflexoes.Select(MapearParaViewModel);
    }

    private static ReflexaoViewModel MapearParaViewModel(Reflexao reflexao)
    {
        return new ReflexaoViewModel
        {
            UsuarioId = reflexao.UsuarioId,
            ReflexaoId = reflexao.Id,
            Conteudo = reflexao.Conteudo,
            DataDeCadastro = reflexao.DataDeCadastro.ToString("G", new CultureInfo("pt-BR")),
            Emocao = reflexao.Emocao != null ? EmocaoViewModel.Mapear(reflexao.Emocao) : null
        };
    }
}