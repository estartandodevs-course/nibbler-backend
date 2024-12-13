using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Nibbler.Core.Mediator;
using Nibbler.Diario.App.Commands;
using Nibbler.Diario.App.InputModels;
using Nibbler.WebApi.Core.Controllers;

namespace Nibbler.WebAPI.Controllers;

/// <summary>
/// Gerencia as entradas individuais de cada diário
/// </summary>
/// <remarks>
/// Este controller permite gerenciar as entradas (posts) dentro de um diário específico.
/// Cada diário pode ter múltiplas entradas, que representam registros individuais
/// feitos em diferentes momentos.
/// 
/// Todas as operações neste controller são contextualizadas a um diário específico,
/// identificado pelo parâmetro diarioId na rota.
/// </remarks>
[ApiController]
[Route("api/diario/{diarioId}/entradas")]
[EnableCors("AllowAll")]
[Produces("application/json")]
[Tags("Entradas")]
public class EntradasController : MainController
{
    private readonly IMediatorHandler _mediatorHandler;

    public EntradasController(IMediatorHandler mediatorHandler)
    {
        _mediatorHandler = mediatorHandler;
    }

    /// <summary>
    /// Adiciona uma nova entrada ao diário
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     POST /api/diario/123e4567-e89b-12d3-a456-426614174000/entradas
    ///     {
    ///         "conteudo": "Hoje foi um dia especial..."
    ///     }
    ///
    /// O conteúdo deve ter entre 10 e 5000 caracteres.
    /// </remarks>
    /// <param name="diarioId">ID do diário que receberá a entrada</param>
    /// <param name="inputModel">Dados da nova entrada</param>
    /// <response code="200">Entrada adicionada com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="404">Diário não encontrado</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Adicionar(Guid diarioId, [FromBody] AdicionarEntradaInputModel inputModel)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var command = new AdicionarEntradaCommand(diarioId, inputModel.Conteudo);
        return CustomResponse(await _mediatorHandler.EnviarComando(command));
    }

    /// <summary>
    /// Atualiza uma entrada existente
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     PUT /api/diario/123e4567-e89b-12d3-a456-426614174000/entradas/987fcdeb-51a2-12d3-a456-426614174000
    ///     {
    ///         "conteudo": "Conteúdo atualizado da entrada..."
    ///     }
    ///
    /// O conteúdo deve ter entre 10 e 5000 caracteres.
    /// </remarks>
    /// <param name="diarioId">ID do diário que contém a entrada</param>
    /// <param name="id">ID da entrada a ser atualizada</param>
    /// <param name="inputModel">Novos dados da entrada</param>
    /// <response code="200">Entrada atualizada com sucesso</response>
    /// <response code="400">Dados inválidos fornecidos</response>
    /// <response code="404">Diário ou entrada não encontrada</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(Guid diarioId, Guid id, [FromBody] AtualizarEntradaInputModel inputModel)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var command = new AtualizarEntradaCommand(diarioId, id, inputModel.Conteudo);
        return CustomResponse(await _mediatorHandler.EnviarComando(command));
    }

    /// <summary>
    /// Remove uma entrada do diário
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(Guid diarioId, Guid id)
    {
        var command = new RemoverEntradaCommand(diarioId, id);
        return CustomResponse(await _mediatorHandler.EnviarComando(command));
    }
}
