namespace Nibbler.Diario.App.ViewModels;

public class EntradaViewModel
{
    public Guid Id { get; set; }
    public string Conteudo { get; set; }
    public DateTime DataDaEntrada { get; set; }
    public DateTime DataDeCadastro { get; set; }
    public DateTime? DataDeAlteracao { get; set; }
}