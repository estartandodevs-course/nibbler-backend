using System.ComponentModel.DataAnnotations;

namespace Nibbler.Diario.App.InputModels;

public class AdicionarDiarioInputModel
{
    [Required(ErrorMessage = "O ID do usuário é obrigatório")]
    public Guid UsuarioId { get; set; }

    [Required(ErrorMessage = "O título é obrigatório")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 150 caracteres")]
    public string Titulo { get; set; }

    [Required(ErrorMessage = "O conteúdo é obrigatório")]
    [StringLength(5000, MinimumLength = 10, ErrorMessage = "O conteúdo deve ter entre 10 e 5000 caracteres")]
    public string Conteudo { get; set; }
}

public class AtualizarDiarioInputModel
{
    [Required(ErrorMessage = "O título é obrigatório")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 150 caracteres")]
    public string Titulo { get; set; }

    [Required(ErrorMessage = "O conteúdo é obrigatório")]
    [StringLength(5000, MinimumLength = 10, ErrorMessage = "O conteúdo deve ter entre 10 e 5000 caracteres")]
    public string Conteudo { get; set; }
}