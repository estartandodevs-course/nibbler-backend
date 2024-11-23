using Dapper;
using Nibbler.Diario.App.ViewModels;
using Nibbler.Diario.Domain.Interfaces;

namespace Nibbler.Diario.App.Queries;

public class EntradaQueries : IEntradaQueries
{
    private readonly IDiarioRepository _diarioRepository;

    public EntradaQueries(IDiarioRepository diarioRepository)
    {
        _diarioRepository = diarioRepository;
    }

    public async Task<IEnumerable<EntradaViewModel>> ObterPorDiario(Guid diarioId)
    {
        const string sql = @"
            SELECT Id, DiarioId, Conteudo, DataDaEntrada, DataDeCadastro, DataDeAlteracao 
            FROM Entradas 
            WHERE DiarioId = @diarioId 
            ORDER BY DataDaEntrada DESC";

        using var conexao = _diarioRepository.ObterConexao();
        return await conexao.QueryAsync<EntradaViewModel>(sql, new { diarioId });
    }

    public async Task<EntradaViewModel> ObterPorId(Guid id)
    {
        const string sql = @"
            SELECT Id, DiarioId, Conteudo, DataDaEntrada, DataDeCadastro, DataDeAlteracao 
            FROM Entradas 
            WHERE Id = @id";

        using var conexao = _diarioRepository.ObterConexao();
        return await conexao.QueryFirstOrDefaultAsync<EntradaViewModel>(sql, new { id });
    }

    public async Task<IEnumerable<EntradaViewModel>> ObterPorPeriodo(Guid diarioId, DateTime inicio, DateTime fim)
    {
        const string sql = @"
            SELECT Id, DiarioId, Conteudo, DataDaEntrada, DataDeCadastro, DataDeAlteracao 
            FROM Entradas 
            WHERE DiarioId = @diarioId 
            AND DataDaEntrada BETWEEN @inicio AND @fim
            ORDER BY DataDaEntrada DESC";

        using var conexao = _diarioRepository.ObterConexao();
        return await conexao.QueryAsync<EntradaViewModel>(sql, new { diarioId, inicio, fim });
    }
}