using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Nibbler.Core.Mediator;
using Nibbler.Diario.app.Commands;
using Nibbler.Diario.App.Commands;
using Nibbler.Diario.App.InputModels;
using Nibbler.Diario.App.Queries;
using Nibbler.Diario.app.Queries.QueriesInterface;
using Nibbler.WebApi.Core.Controllers;

namespace Nibbler.WebAPI.Controllers;

/// <summary>
/// Gerencia o catálogo de emoções disponíveis no sistema
/// </summary>
/// <remarks>
/// Este controller permite:
/// - Criar e gerenciar emoções
/// - Consultar emoções disponíveis
/// - Associar emoções a reflexões
/// 
/// As emoções são utilizadas para categorizar reflexões,
/// permitindo um acompanhamento emocional do usuário.
/// </remarks>
[ApiController]
[Route("api/emocoes")]
[EnableCors("PermissoesDeOrigem")]
[Produces("application/json")]
[Tags("Emoções")]
public class EmocaoController : MainController
{
    private readonly IMediatorHandler _mediatorHandler;
    private readonly IEmocaoQueries _emocaoQueries;

    public EmocaoController(
        IMediatorHandler mediatorHandler,
        IEmocaoQueries emocaoQueries)
    {
        _mediatorHandler = mediatorHandler;
        _emocaoQueries = emocaoQueries;
    }

    /// <summary>
    /// Lista todas as emoções disponíveis
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     GET /api/emocoes
    ///
    /// Retorna todas as emoções cadastradas no sistema.
    /// </remarks>
    /// <response code="200">Lista de emoções retornada com sucesso</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterTodas()
    {
        var emocoes = await _emocaoQueries.ObterTodas();
        return CustomResponse(emocoes);
    }

    /// <summary>
    /// Obtém uma emoção específica pelo seu ID
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     GET /api/emocoes/123e4567-e89b-12d3-a456-426614174000
    ///
    /// Retorna os detalhes de uma emoção específica.
    /// </remarks>
    /// <param name="id">ID da emoção</param>
    /// <response code="200">Emoção encontrada e retornada com sucesso</response>
    /// <response code="404">Emoção não encontrada</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var emocao = await _emocaoQueries.ObterPorId(id);
        return emocao == null ? NotFound() : CustomResponse(emocao);
    }

    /// <summary>
    /// Cria uma nova emoção
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     POST /api/emocoes
    ///     {
    ///         "nome": "Felicidade"
    ///     }
    ///
    /// O nome da emoção deve ter entre 2 e 50 caracteres.
    /// </remarks>
    /// <param name="inputModel">Dados da nova emoção</param>
    /// <response code="200">Emoção criada com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Adicionar([FromBody] AdicionarEmocaoInputModel inputModel)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var command = new AdicionarEmocaoCommand(inputModel.Nome);
        return CustomResponse(await _mediatorHandler.EnviarComando(command));
    }

    /// <summary>
    /// Atualiza uma emoção existente
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     PUT /api/emocoes/123e4567-e89b-12d3-a456-426614174000
    ///     {
    ///         "nome": "Alegria"
    ///     }
    ///
    /// O nome da emoção deve ter entre 2 e 50 caracteres.
    /// </remarks>
    /// <param name="id">ID da emoção</param>
    /// <param name="inputModel">Novos dados da emoção</param>
    /// <response code="200">Emoção atualizada com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="404">Emoção não encontrada</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarEmocaoInputModel inputModel)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var command = new AtualizarEmocaoCommand(id, inputModel.Nome);
        return CustomResponse(await _mediatorHandler.EnviarComando(command));
    }

    /// <summary>
    /// Exclui uma emoção
    /// </summary>
    /// <remarks>
    /// A exclusão só é permitida se a emoção não estiver associada a nenhuma reflexão.
    /// </remarks>
    /// <param name="id">ID da emoção</param>
    /// <response code="200">Emoção excluída com sucesso</response>
    /// <response code="400">Erro na requisição ou emoção em uso</response>
    /// <response code="404">Emoção não encontrada</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var command = new ExcluirEmocaoCommand(id);
        return CustomResponse(await _mediatorHandler.EnviarComando(command));
    }
}