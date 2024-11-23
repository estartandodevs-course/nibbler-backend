
using Nibbler.Diario.App.ViewModels;
using Nibbler.Diario.Domain.Interfaces;
using System.Globalization;
using Nibbler.Diario.app.Queries.QueriesInterface;

namespace Nibbler.Diario.App.Queries;

public class DiarioQueries : IDiarioQueries
{
    private readonly IDiarioRepository _diarioRepository;

    public DiarioQueries(IDiarioRepository diarioRepository)
    {
        _diarioRepository = diarioRepository;
    }

    public async Task<IEnumerable<DiarioViewModel>> ObterTodosAtivos()
    {
        var diarios = await _diarioRepository.ObterTodosAtivos();
        return diarios.Select(MapearParaViewModel);
    }

    public async Task<IEnumerable<DiarioViewModel>> ObterTodosInativos()
    {
        var diarios = await _diarioRepository.ObterTodosInativos();
        return diarios.Select(MapearParaViewModel);
    }
    


    public async Task<IEnumerable<EntradaViewModel>> ObterEntradasPorDiario(Guid diarioId)
    {
        var entradas = await _diarioRepository.ObterEntradasPorDiario(diarioId);
        return entradas.Select(e => new EntradaViewModel
        {
            Id = e.Id,
            Conteudo = e.Conteudo
        });
    }

    public async Task<EntradaViewModel> ObterEntradaPorId(Guid diarioId, Guid entradaId)
    {
        var entrada = await _diarioRepository.ObterEntradaPorId(diarioId, entradaId);
        if (entrada == null) return null;
        
        return new EntradaViewModel
        {
            Id = entrada.Id,
            Conteudo = entrada.Conteudo
        };
    }

    public async Task<DiarioViewModel> ObterPorId(Guid id)
    {
        var diario = await _diarioRepository.ObterPorId(id);
        return diario == null ? null : MapearParaViewModel(diario);
    }

    public async Task<IEnumerable<DiarioViewModel>> ObterPorUsuario(Guid usuarioId)
    {
        var diarios = await _diarioRepository.ObterPorUsuario(usuarioId);
        return diarios.Select(MapearParaViewModel);
    }

    public async Task<IEnumerable<DiarioViewModel>> ObterPorEtiqueta(string etiqueta)
    {
        var diarios = await _diarioRepository.ObterPorEtiqueta(etiqueta);
        return diarios.Select(MapearParaViewModel);
    }

    private static DiarioViewModel MapearParaViewModel(Domain.Diario diario)
    {
        return new DiarioViewModel
        {
            Id = diario.Id,
            Titulo = diario.Titulo,
            Conteudo = diario.Conteudo,
            DataDeCadastro = diario.DataDeCadastro.ToString("G", new CultureInfo("pt-BR")),
            DataDeAlteracao = diario.DataDeAlteracao.ToString("G", new CultureInfo("pt-BR")),
            QuantidadeReflexoes = diario.QuantidadeReflexoes,
            Usuario = new UsuarioResumoViewModel
            {
                Id = diario.Usuario.Id,
                Nome = diario.Usuario.Nome,
                CaminhoFoto = diario.Usuario.CaminhoFoto
            },
            Etiquetas = diario.Etiquetas?.Select(e => new EtiquetaViewModel
            {
                Id = e.Id,
                Nome = e.Nome
            }).ToList()
        };
    }
}