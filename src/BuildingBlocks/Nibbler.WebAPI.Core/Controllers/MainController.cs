using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace Nibbler.WebApi.Core.Controllers;

[ApiController]
public abstract class MainController : ControllerBase
{
    protected ICollection<string> Erros = new List<string>();

    protected ActionResult CustomResponse(object result = null)
    {
        if (OperacaoValida())
        {
            return Ok(new
            {
                sucess = true,
                data = result
            });
        }

        return BadRequest(new
        {
            sucess = false,
            erros = Erros.ToArray()
        });
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
        var erros = modelState.Values.SelectMany(e => e.Errors);
        foreach (var erro in erros)
        {
            AdicionarErro(erro.ErrorMessage);
        }

        return CustomResponse();
    }


    protected ActionResult CustomResponse(ValidationResult validationResult)
    {
        foreach (var erro in validationResult.Errors)
        {
            AdicionarErro(erro.ErrorMessage);
        }

        return CustomResponse();
    }


    protected bool OperacaoValida() => !Erros.Any();

    protected void AdicionarErro(string erro) => Erros.Add(erro);

    protected void LimparErros() => Erros.Clear();

}