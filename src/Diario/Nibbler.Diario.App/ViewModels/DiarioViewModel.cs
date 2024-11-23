using System.Globalization;
using Nibbler.Diario.Domain;

namespace Nibbler.Diario.App.ViewModels;

public class DiarioViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public string Conteudo { get; set; }
    public string DataDeCadastro { get; set; }
    public string DataDeAlteracao { get; set; }
    public int QuantidadeReflexoes { get; set; }
    public IEnumerable<EtiquetaViewModel> Etiquetas { get; set; }
    public UsuarioResumoViewModel Usuario { get; set; }

    public static DiarioViewModel Mapear(Domain.Diario diario)
    {
        return new DiarioViewModel
        {
            Id = diario.Id,
            Titulo = diario.Titulo,
            Conteudo = diario.Conteudo,
            DataDeCadastro = diario.DataDeCadastro.ToString("G", new CultureInfo("pt-BR")),
            DataDeAlteracao = diario.DataDeAlteracao.ToString("G", new CultureInfo("pt-BR")),
            QuantidadeReflexoes = diario.QuantidadeReflexoes,
            Etiquetas = diario.Etiquetas.Select(EtiquetaViewModel.Mapear),
            Usuario = UsuarioResumoViewModel.Mapear(diario.Usuario)
        };
    }
}