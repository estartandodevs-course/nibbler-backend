using System.Globalization;
using Nibbler.Diario.Domain;

namespace Nibbler.Diario.App.ViewModels;

public class ReflexaoViewModel
{
    public Guid UsuarioId { get; set; }
    public string Nome { get; set; }
    public string Foto { get; set; }
    public string Descricao { get; set; }
    public string DataDeCadastro { get; set; }

    public static ReflexaoViewModel Mapear(Reflexao reflexao)
    {
        return new ReflexaoViewModel
        {
            UsuarioId = reflexao.Usuario.Id,
            Nome = reflexao.Usuario.Nome,
            Foto = reflexao.Usuario.CaminhoFoto,
            Descricao = reflexao.Descricao,
            DataDeCadastro = reflexao.DataDeCadastro.ToString("G", new CultureInfo("pt-BR"))
        };
    }
}