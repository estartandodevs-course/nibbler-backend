using System.ComponentModel.DataAnnotations;

namespace Nibbler.Diario.App.InputModels;

public class AdicionarReflexaoInputModel
{
    [Required(ErrorMessage = "O ID do usuário é obrigatório")]
    public Guid UsuarioId { get; set; }

    [Required(ErrorMessage = "O conteúdo é obrigatório")]
    [StringLength(5000, MinimumLength = 10, ErrorMessage = "O conteúdo deve ter entre 10 e 5000 caracteres")]
    public string Conteudo { get; set; }

    public Guid? EmocaoId { get; set; }
}

public class AtualizarReflexaoInputModel
{
    [Required(ErrorMessage = "O conteúdo é obrigatório")]
    [StringLength(5000, MinimumLength = 10, ErrorMessage = "O conteúdo deve ter entre 10 e 5000 caracteres")]
    public string Conteudo { get; set; }

    public Guid? EmocaoId { get; set; }
}