namespace Nibbler.Diario.App.ViewModels;

public class UsuarioResumoViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string CaminhoFoto { get; set; }

    public static UsuarioResumoViewModel Mapear(Domain.Usuario usuario)
    {
        return new UsuarioResumoViewModel
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            CaminhoFoto = usuario.CaminhoFoto
        };
    }
}