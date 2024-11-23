using System.ComponentModel.DataAnnotations;

namespace Nibbler.Diario.App.InputModels;

public class AdicionarEntradaInputModel
{
    [Required(ErrorMessage = "O conteúdo é obrigatório")]
    [StringLength(5000, MinimumLength = 10, ErrorMessage = "O conteúdo deve ter entre 10 e 5000 caracteres")]
    public string Conteudo { get; set; }
}

public class AtualizarEntradaInputModel
{
    [Required(ErrorMessage = "O conteúdo é obrigatório")]
    [StringLength(5000, MinimumLength = 10, ErrorMessage = "O conteúdo deve ter entre 10 e 5000 caracteres")]
    public string Conteudo { get; set; }
}