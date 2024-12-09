using System.Globalization;
using Nibbler.Diario.Domain;

namespace Nibbler.Diario.App.ViewModels;

public class ReflexaoViewModel
{
    public Guid UsuarioId { get; set; }
    public Guid ReflexaoId { get; set; }
    public string Conteudo { get; set; }
    public string DataDeCadastro { get; set; }
    public EmocaoViewModel Emocao { get; set; }

    public static ReflexaoViewModel Mapear(Reflexao reflexao)
    {
        return new ReflexaoViewModel
        {
            UsuarioId = reflexao.UsuarioId,
            ReflexaoId = reflexao.Id,
            Conteudo = reflexao.Conteudo,
            DataDeCadastro = reflexao.DataDeCadastro.ToString("G", new CultureInfo("pt-BR")),
            Emocao = EmocaoViewModel.Mapear(reflexao.Emocao)
        };
    }
}