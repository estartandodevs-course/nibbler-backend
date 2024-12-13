using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Nibbler.Core.Mediator;
using Nibbler.Diario.app.Commands;
using Nibbler.Diario.App.Commands;
using Nibbler.Diario.App.InputModels;
using Nibbler.Diario.app.Queries.QueriesInterface;
using Nibbler.WebApi.Core.Controllers;

namespace Nibbler.WebAPI.Controllers;

/// <summary>
/// Gerencia as reflexões pessoais dos usuários
/// </summary>
/// <remarks>
/// Este controller permite:
/// - Criar e gerenciar reflexões
/// - Associar emoções às reflexões
/// - Consultar reflexões por diferentes critérios
/// 
/// As reflexões são registros pessoais que podem estar associados a emoções,
/// permitindo um acompanhamento emocional do usuário.
/// </remarks>
[ApiController]
[Route("api/reflexoes")]
[EnableCors("AllowAll")]
[Produces("application/json")]
[Tags("Reflexões")]
public class ReflexaoController : MainController
{
    private readonly IMediatorHandler _mediatorHandler;
    private readonly IReflexaoQueries _reflexaoQueries;

    public ReflexaoController(
        IMediatorHandler mediatorHandler,
        IReflexaoQueries reflexaoQueries)
    {
        _mediatorHandler = mediatorHandler;
        _reflexaoQueries = reflexaoQueries;
    }

    /// <summary>
    /// Lista todas as reflexões de um usuário
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     GET /api/reflexoes/usuario/123e4567-e89b-12d3-a456-426614174000
    ///
    /// Retorna todas as reflexões do usuário, ordenadas por data de criação.
    /// </remarks>
    /// <param name="usuarioId">ID do usuário</param>
    /// <response code="200">Lista de reflexões retornada com sucesso</response>
    [HttpGet("usuario/{usuarioId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterPorUsuario(Guid usuarioId)
    {
        var reflexoes = await _reflexaoQueries.ObterPorUsuario(usuarioId);
        return CustomResponse(reflexoes);
    }

    /// <summary>
    /// Obtém uma reflexão específica pelo seu ID
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     GET /api/reflexoes/123e4567-e89b-12d3-a456-426614174000
    ///
    /// Retorna os detalhes completos de uma reflexão, incluindo sua emoção associada.
    /// </remarks>
    /// <param name="id">ID da reflexão</param>
    /// <response code="200">Reflexão encontrada e retornada com sucesso</response>
    /// <response code="404">Reflexão não encontrada</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var reflexao = await _reflexaoQueries.ObterPorId(id);
        return reflexao == null ? NotFound() : CustomResponse(reflexao);
    }

    /// <summary>
    /// Cria uma nova reflexão
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     POST /api/reflexoes
    ///     {
    ///         "usuarioId": "123e4567-e89b-12d3-a456-426614174000",
    ///         "conteudo": "Hoje me senti muito bem...",
    ///         "emocaoId": "987fcdeb-51a2-12d3-a456-426614174000"
    ///     }
    ///
    /// O conteúdo deve ter entre 10 e 5000 caracteres.
    /// A emoção é opcional.
    /// </remarks>
    /// <param name="inputModel">Dados da nova reflexão</param>
    /// <response code="200">Reflexão criada com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Adicionar([FromBody] AdicionarReflexaoInputModel inputModel)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var command = new AdicionarReflexaoCommand(
            inputModel.UsuarioId,
            inputModel.Conteudo,
            inputModel.EmocaoId);

        return CustomResponse(await _mediatorHandler.EnviarComando(command));
    }

    /// <summary>
    /// Atualiza uma reflexão existente
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     PUT /api/reflexoes/123e4567-e89b-12d3-a456-426614174000
    ///     {
    ///         "conteudo": "Conteúdo atualizado...",
    ///         "emocaoId": "987fcdeb-51a2-12d3-a456-426614174000"
    ///     }
    ///
    /// O conteúdo deve ter entre 10 e 5000 caracteres.
    /// A emoção pode ser alterada ou removida (null).
    /// </remarks>
    /// <param name="id">ID da reflexão</param>
    /// <param name="inputModel">Novos dados da reflexão</param>
    /// <response code="200">Reflexão atualizada com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="404">Reflexão não encontrada</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarReflexaoInputModel inputModel)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var command = new AtualizarReflexaoCommand(id, inputModel.Conteudo, inputModel.EmocaoId);
        return CustomResponse(await _mediatorHandler.EnviarComando(command));
    }

    /// <summary>
    /// Exclui uma reflexão
    /// </summary>
    /// <param name="id">ID da reflexão</param>
    /// <response code="200">Reflexão excluída com sucesso</response>
    /// <response code="400">Erro na requisição</response>
    /// <response code="404">Reflexão não encontrada</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var command = new ExcluirReflexaoCommand(id);
        return CustomResponse(await _mediatorHandler.EnviarComando(command));
    }
}