using Nibbler.Diario.Domain;

namespace Nibbler.Diario.App.ViewModels;

public class EtiquetaViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }

    public static EtiquetaViewModel Mapear(Etiqueta etiqueta)
    {
        return new EtiquetaViewModel
        {
            Id = etiqueta.Id,
            Nome = etiqueta.Nome
        };
    }
}