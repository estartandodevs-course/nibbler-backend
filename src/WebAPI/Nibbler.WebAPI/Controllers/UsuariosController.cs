using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
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
[EnableCors("PermissoesDeOrigem")]
[Produces("application/json")]
[Tags("Usuários")]
public class UsuariosController : MainController
{
    public readonly IUsuarioQueries _usuarioQueries;

    public readonly IMediatorHandler _mediatorHandler;
    
    private readonly UserManager<IdentityUser> _userManager;

    public UsuariosController(IMediatorHandler mediatorHandler, IUsuarioQueries usuarioQueries, UserManager<IdentityUser> userManager)
    {
        
        _mediatorHandler = mediatorHandler;
        _usuarioQueries = usuarioQueries;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos()
    {
        var usuarios = await _usuarioQueries.ObterTodos();

        return CustomResponse(usuarios);
    }

    [HttpPut("{Id:Guid}")]
    public async Task<IActionResult> Atualizar(Guid Id, AtualizarUsuarioInputModel model)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var dataDeNascimento = model.DataDeNascimento.ConverterParaData();

        if (dataDeNascimento is null)
        {
            AdicionarErro("Data de nascimento inválida!");
            return CustomResponse();
        }

        var comando = new AtualizarUsuarioCommand(Id,model.Nome, dataDeNascimento!.Value, model.Foto);

        var result = await _mediatorHandler.EnviarComando(comando);

        return CustomResponse(result);

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
        var user = new IdentityUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = model.Email,
            NormalizedUserName = model.Email.ToUpper(),
            Email = model.Email,
            NormalizedEmail = model.Email.ToUpper(),
            EmailConfirmed = true
        };

        var identidadeCriada = await _userManager.CreateAsync(user, model.Senha);

        if (!identidadeCriada.Succeeded)
        {
            foreach (var erro in identidadeCriada.Errors) AdicionarErro(erro.Description);

            return CustomResponse();
        }
        var comando = new AdicionarUsuarioCommand(model.Nome, dataDeNascimento!.Value, model.Foto, model.Email, model.Senha);

        var result = await _mediatorHandler.EnviarComando(comando);

        return CustomResponse(result);
    }
    
}
