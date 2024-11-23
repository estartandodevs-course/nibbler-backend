using Microsoft.AspNetCore.Mvc;
using Nibbler.Core.Mediator;
using Nibbler.Diario.app.Commands;
using Nibbler.Diario.App.Commands;
using Nibbler.Diario.App.InputModels;
using Nibbler.Diario.App.Queries;
using Nibbler.WebApi.Core.Controllers;

namespace Nibbler.WebAPI.Controllers
{
    /// <summary>
    /// Gerencia as operações relacionadas às etiquetas dos diários
    /// </summary>
    /// <remarks>
    /// Este controller permite:
    /// - Criar e gerenciar etiquetas independentes
    /// - Associar etiquetas a diários
    /// - Consultar etiquetas por diferentes critérios
    /// - Remover etiquetas de diários
    /// 
    /// As etiquetas são uma forma de categorizar e organizar os diários,
    /// facilitando sua busca e agrupamento.
    /// </remarks>
    [Route("api/etiquetas")]
    [ApiController]
    [Produces("application/json")]
    [Tags("Etiquetas")]
    public class EtiquetasController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IEtiquetaQueries _etiquetaQueries;

        public EtiquetasController(IMediatorHandler mediatorHandler, IEtiquetaQueries etiquetaQueries)
        {
            _mediatorHandler = mediatorHandler;
            _etiquetaQueries = etiquetaQueries;
        }

        /// <summary>
        /// Lista todas as etiquetas disponíveis no sistema
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /api/etiquetas
        ///
        /// Este endpoint retorna todas as etiquetas cadastradas, independente de estarem
        /// associadas a diários ou não.
        /// </remarks>
        /// <response code="200">Lista de etiquetas retornada com sucesso</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTodas()
        {
            var etiquetas = await _etiquetaQueries.ObterTodas();
            return CustomResponse(etiquetas);
        }

        /// <summary>
        /// Obtém uma etiqueta específica pelo seu ID
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /api/etiquetas/123e4567-e89b-12d3-a456-426614174000
        ///
        /// Este endpoint retorna os detalhes de uma etiqueta específica.
        /// </remarks>
        /// <param name="id">ID único da etiqueta (GUID)</param>
        /// <response code="200">Etiqueta encontrada e retornada com sucesso</response>
        /// <response code="404">Etiqueta não encontrada no sistema</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var etiqueta = await _etiquetaQueries.ObterPorId(id);
            if (etiqueta == null) return NotFound();
            return CustomResponse(etiqueta);
        }

        /// <summary>
        /// Cria uma nova etiqueta no sistema
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     POST /api/etiquetas
        ///     {
        ///         "nome": "Importante"
        ///     }
        ///
        /// O nome da etiqueta deve ter entre 2 e 50 caracteres.
        /// </remarks>
        /// <param name="inputModel">Dados da nova etiqueta</param>
        /// <response code="200">Etiqueta criada com sucesso</response>
        /// <response code="400">Dados inválidos fornecidos</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Criar([FromBody] AdicionarEtiquetaInputModel inputModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var command = new CriarEtiquetaCommand(inputModel.Nome);
            return CustomResponse(await _mediatorHandler.EnviarComando(command));
        }

        /// <summary>
        /// Atualiza uma etiqueta existente
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     PUT /api/etiquetas/123e4567-e89b-12d3-a456-426614174000
        ///     {
        ///         "nome": "Muito Importante"
        ///     }
        ///
        /// O nome da etiqueta deve ter entre 2 e 50 caracteres.
        /// </remarks>
        /// <param name="id">ID da etiqueta a ser atualizada</param>
        /// <param name="inputModel">Novos dados da etiqueta</param>
        /// <response code="200">Etiqueta atualizada com sucesso</response>
        /// <response code="400">Dados inválidos fornecidos</response>
        /// <response code="404">Etiqueta não encontrada</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarEtiquetaInputModel inputModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var command = new AtualizarEtiquetaCommand(id, inputModel.Nome);
            return CustomResponse(await _mediatorHandler.EnviarComando(command));
        }
        
        /// <summary>
        /// Adiciona uma etiqueta existente a um diário
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     POST /api/etiquetas/diario/123e4567-e89b-12d3-a456-426614174000/existente/987fcdeb-51a2-12d3-a456-426614174000
        ///
        /// Este endpoint permite associar uma etiqueta já existente a um diário específico.
        /// A etiqueta e o diário devem existir previamente no sistema.
        /// Uma etiqueta pode ser associada a múltiplos diários.
        /// </remarks>
        /// <param name="diarioId">ID do diário que receberá a etiqueta</param>
        /// <param name="etiquetaId">ID da etiqueta a ser adicionada ao diário</param>
        /// <response code="200">Etiqueta adicionada com sucesso ao diário</response>
        /// <response code="400">Erro na requisição - IDs inválidos</response>
        /// <response code="404">Diário ou etiqueta não encontrado no sistema</response>
        [HttpPost("diario/{diarioId}/existente/{etiquetaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AdicionarEtiquetaExistenteAoDiario(Guid diarioId, Guid etiquetaId)
        {
            var command = new AdicionarEtiquetaAoDiarioCommand(diarioId, etiquetaId);
            return CustomResponse(await _mediatorHandler.EnviarComando(command));
        }

        /// <summary>
        /// Exclui uma etiqueta
        /// </summary>
        /// <param name="id">ID da etiqueta</param>
        /// <response code="200">Etiqueta excluída com sucesso</response>
        /// <response code="400">Erro na requisição</response>
        /// <response code="404">Etiqueta não encontrada</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Excluir(Guid id)
        {
            return CustomResponse(await _mediatorHandler.EnviarComando(new ExcluirEtiquetaCommand(id)));
        }
    }
}