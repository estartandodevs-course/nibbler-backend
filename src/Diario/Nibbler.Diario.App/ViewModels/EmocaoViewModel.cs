using Nibbler.Diario.Domain;

namespace Nibbler.Diario.App.ViewModels;

public class EmocaoViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }

    public static EmocaoViewModel Mapear(Emocao emocao)
    {
        return new EmocaoViewModel
        {
            Id = emocao.Id,
            Nome = emocao.Nome
        };
    }
}