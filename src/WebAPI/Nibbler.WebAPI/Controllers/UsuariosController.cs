using Nibbler.Core.Mediator;
using Nibbler.Core.Utilities;
using Nibbler.Usuario.App.Commands;
using Nibbler.Usuario.App.Queries;
using Microsoft.AspNetCore.Mvc;
using Nibbler.WebApi.Core.Controllers;
using Nibbler.WebAPI.InputModels;

namespace Nibbler.WebAPI.Controllers;

[ApiController]
[Route("api/usuario")]
[Produces("application/json")]
[Tags("Usuários")]
public class UsuariosController : MainController
{
    public readonly IUsuarioQueries _usuarioQueries;

    public readonly IMediatorHandler _mediatorHandler;

    public UsuariosController(IMediatorHandler mediatorHandler, IUsuarioQueries usuarioQueries)
    {

        _mediatorHandler = mediatorHandler;
        _usuarioQueries = usuarioQueries;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos()
    {
        var usuarios = await _usuarioQueries.ObterTodos();

        return CustomResponse(usuarios);
    }


    [HttpPost]
    public async Task<IActionResult> Adicionar(UsuarioInputModel model)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var dataDeNascimento = model.DataDeNascimento.ConverterParaData();

        if (dataDeNascimento is null)
        {
            AdicionarErro("Data de nascimento inválida!");
            return CustomResponse();
        }

        var comando = new AdicionarUsuarioCommand(model.Nome, dataDeNascimento!.Value, model.Foto, model.Email, model.Senha);

        var result = await _mediatorHandler.EnviarComando(comando);

        return CustomResponse(result);
    }
    
}
