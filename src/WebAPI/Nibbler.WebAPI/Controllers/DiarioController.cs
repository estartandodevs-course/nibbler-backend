using Microsoft.AspNetCore.Mvc;
using Nibbler.Core.Mediator;
using Nibbler.Diario.App.Commands;
using Nibbler.Diario.App.Queries;
using Nibbler.WebApi.Core.Controllers;
using Nibbler.Diario.App.InputModels;
using Nibbler.Diario.app.Queries.QueriesInterface;

namespace Nibbler.WebAPI.Controllers
{
    /// <summary>
    /// Gerencia operações relacionadas aos diários pessoais
    /// </summary>
    /// <remarks>
    /// Este controller permite gerenciar o ciclo de vida completo dos diários, incluindo:
    /// - Criação e atualização de diários
    /// - Consulta de diários por diferentes critérios
    /// - Exclusão lógica (soft delete) e permanente
    /// - Gerenciamento de etiquetas associadas
    /// - Filtros por status (ativos/inativos)
    /// - Buscas por usuário e etiquetas
    /// 
    /// Todos os endpoints retornam respostas no formato JSON e seguem o padrão REST.
    /// As operações de escrita (POST, PUT, DELETE) requerem autenticação.
    /// </remarks>
    [ApiController]
    [Route("api/diario")]
    [Produces("application/json")]
    [Tags("Diários")]
    public class DiarioController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IDiarioQueries _diarioQueries;

        public DiarioController(IMediatorHandler mediatorHandler, IDiarioQueries diarioQueries)
        {
            _mediatorHandler = mediatorHandler;
            _diarioQueries = diarioQueries;
        }

        /// <summary>
        /// Adiciona um novo diário
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     POST /api/diario
        ///     {
        ///         "usuarioId": "123e4567-e89b-12d3-a456-426614174000",
        ///         "titulo": "Meu Diário",
        ///         "conteudo": "Conteúdo inicial do diário"
        ///     }
        ///
        /// Regras de validação:
        /// - O título deve ter entre 3 e 150 caracteres
        /// - O conteúdo deve ter entre 10 e 5000 caracteres
        /// - O usuário deve existir no sistema
        /// 
        /// O diário será criado com status ativo e poderá receber etiquetas posteriormente.
        /// </remarks>
        /// <param name="inputModel">Dados do diário a ser criado</param>
        /// <response code="200">Diário criado com sucesso</response>
        /// <response code="400">Dados inválidos fornecidos</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Adicionar([FromBody] AdicionarDiarioInputModel inputModel)
        {
            var command = new AdicionarDiarioCommand(inputModel.UsuarioId, inputModel.Titulo, inputModel.Conteudo);
            var result = await _mediatorHandler.EnviarComando(command);
            return CustomResponse(result);
        }

        /// <summary>
        /// Atualiza um diário existente
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     PUT /api/diario/123e4567-e89b-12d3-a456-426614174000
        ///     {
        ///         "titulo": "Meu Diário Atualizado",
        ///         "conteudo": "Novo conteúdo do diário..."
        ///     }
        ///
        /// Regras de validação:
        /// - O título deve ter entre 3 e 150 caracteres
        /// - O conteúdo deve ter entre 10 e 5000 caracteres
        /// - O diário deve existir e estar ativo
        /// - Apenas o proprietário pode atualizar o diário
        /// 
        /// A atualização não afeta:
        /// - Etiquetas associadas
        /// - Entradas existentes
        /// - Data de criação
        /// </remarks>
        /// <param name="id">ID do diário a ser atualizado</param>
        /// <param name="inputModel">Novos dados do diário</param>
        /// <response code="200">Diário atualizado com sucesso</response>
        /// <response code="400">Dados inválidos fornecidos</response>
        /// <response code="404">Diário não encontrado</response>
        /// <response code="403">Usuário não autorizado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarDiarioInputModel inputModel)
        {
            var command = new AtualizarDiarioCommand(id, inputModel.Titulo, inputModel.Conteudo);
            
            var result = await _mediatorHandler.EnviarComando(command);
            return CustomResponse(result);
        }

        /// <summary>
        /// Marca um diário como excluído (soft delete)
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     DELETE /api/diario/123e4567-e89b-12d3-a456-426614174000
        ///
        /// Esta operação:
        /// - Não exclui permanentemente o diário
        /// - Mantém todos os dados e relacionamentos
        /// - Remove o diário das listagens padrão
        /// - Permite recuperação posterior
        /// - Mantém histórico de alterações
        /// 
        /// Apenas o proprietário pode excluir o diário.
        /// </remarks>
        /// <param name="id">ID do diário a ser marcado como excluído</param>
        /// <response code="200">Diário marcado como excluído com sucesso</response>
        /// <response code="400">Erro na requisição</response>
        /// <response code="404">Diário não encontrado</response>
        /// <response code="403">Usuário não autorizado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> MarcarComoExcluido(Guid id)
        {
            var command = new MarcarComoExcluidoCommand(id);
            var result = await _mediatorHandler.EnviarComando(command);
            return CustomResponse(result);
        }

        /// <summary>
        /// Obtém um diário pelo ID
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /api/diario/123e4567-e89b-12d3-a456-426614174000
        ///
        /// Retorna os detalhes completos do diário:
        /// - Título e conteúdo
        /// - Data de criação e última atualização
        /// - Status atual (ativo/inativo)
        /// - Lista de etiquetas associadas
        /// - Informações do usuário proprietário
        /// - Lista de entradas ordenadas por data
        /// - Metadados do sistema
        /// </remarks>
        /// <param name="id">ID do diário a ser consultado</param>
        /// <response code="200">Retorna o diário solicitado</response>
        /// <response code="404">Diário não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var diario = await _diarioQueries.ObterPorId(id);
            return CustomResponse(diario);
        }

        /// <summary>
        /// Obtém todos os diários de um usuário
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /api/diario/usuario/123e4567-e89b-12d3-a456-426614174000
        ///
        /// Retorna uma lista paginada com:
        /// - Diários ativos e inativos
        /// - Ordenados por data de atualização
        /// - Informações resumidas de cada diário
        /// - Contagem de entradas
        /// - Lista de etiquetas
        /// - Status de cada diário
        /// 
        /// Suporta paginação via query parameters:
        /// - page: número da página (default: 1)
        /// - size: itens por página (default: 10)
        /// </remarks>
        /// <param name="usuarioId">ID do usuário proprietário dos diários</param>
        /// <response code="200">Lista de diários do usuário</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpGet("usuario/{usuarioId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorUsuario(Guid usuarioId)
        {
            var diarios = await _diarioQueries.ObterPorUsuario(usuarioId);
            return CustomResponse(diarios);
        }

        /// <summary>
        /// Obtém todos os diários ativos
        /// </summary>
        /// <remarks>
        /// Retorna apenas diários:
        /// - Não marcados como excluídos
        /// - Ordenados por data de atualização
        /// - Com informações resumidas
        /// 
        /// Suporta filtros via query parameters:
        /// - data: filtra por data de criação
        /// - etiqueta: filtra por etiqueta
        /// - termo: busca em título e conteúdo
        /// </remarks>
        /// <response code="200">Lista de diários ativos</response>
        [HttpGet("ativos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTodosAtivos()
        {
            var diarios = await _diarioQueries.ObterTodosAtivos();
            return CustomResponse(diarios);
        }

        /// <summary>
        /// Obtém diários por etiqueta
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /api/diario/etiqueta/Importante
        ///
        /// Características:
        /// - Busca case-insensitive
        /// - Retorna apenas diários ativos
        /// - Suporta paginação
        /// - Ordenação por relevância
        /// - Permite múltiplas etiquetas (separadas por vírgula)
        /// </remarks>
        /// <param name="etiqueta">Nome da etiqueta para filtrar os diários</param>
        /// <response code="200">Lista de diários com a etiqueta especificada</response>
        [HttpGet("etiqueta/{etiqueta}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterPorEtiqueta(string etiqueta)
        {
            var diarios = await _diarioQueries.ObterPorEtiqueta(etiqueta);
            return CustomResponse(diarios);
        }

        /// <summary>
        /// Exclui permanentemente um diário
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     DELETE /api/diario/excluir/123e4567-e89b-12d3-a456-426614174000
        ///
        /// Esta operação:
        /// - Remove permanentemente o diário
        /// - Exclui todas as entradas associadas
        /// - Remove associações com etiquetas
        /// - Não pode ser desfeita
        /// 
        /// Requer privilégios administrativos.
        /// </remarks>
        /// <param name="id">ID do diário</param>
        /// <response code="200">Diário excluído com sucesso</response>
        /// <response code="400">Erro na requisição</response>
        /// <response code="404">Diário não encontrado</response>
        /// <response code="403">Usuário não autorizado</response>
        [HttpDelete("excluir/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ExcluirPermanentemente(Guid id)
        {
            var command = new ExcluirDiarioCommand(id);
            var result = await _mediatorHandler.EnviarComando(command);
            return CustomResponse(result);
        }

        /// <summary>
        /// Obtém todos os diários inativos (excluídos logicamente)
        /// </summary>
        /// <remarks>
        /// Retorna diários:
        /// - Marcados como excluídos
        /// - Com data de exclusão
        /// - Ordenados por data de exclusão
        /// - Com informações completas
        /// 
        /// Requer privilégios administrativos.
        /// </remarks>
        /// <response code="200">Lista de diários inativos</response>
        /// <response code="403">Usuário não autorizado</response>
        [HttpGet("inativos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObterTodosInativos()
        {
            var diarios = await _diarioQueries.ObterTodosInativos();
            return CustomResponse(diarios);
        }
    }
}