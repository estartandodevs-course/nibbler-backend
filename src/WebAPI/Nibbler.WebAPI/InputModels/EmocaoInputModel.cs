using System.ComponentModel.DataAnnotations;

namespace Nibbler.Diario.App.InputModels;

public class AdicionarEmocaoInputModel
{
    [Required(ErrorMessage = "O nome da emoção é obrigatório")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 50 caracteres")]
    public string Nome { get; set; }
}

public class AtualizarEmocaoInputModel
{
    [Required(ErrorMessage = "O nome da emoção é obrigatório")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 50 caracteres")]
    public string Nome { get; set; }
}