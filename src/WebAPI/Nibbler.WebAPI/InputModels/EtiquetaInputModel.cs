using System.ComponentModel.DataAnnotations;

namespace Nibbler.Diario.App.InputModels;

public class AdicionarEtiquetaInputModel
{
    [Required(ErrorMessage = "O nome da etiqueta é obrigatório")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 50 caracteres")]
    public string Nome { get; set; }
}

public class AtualizarEtiquetaInputModel
{
    [Required(ErrorMessage = "O nome da etiqueta é obrigatório")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 50 caracteres")]
    public string Nome { get; set; }
}