using Nibbler.Core.DomainObjects;

namespace Nibbler.Diario.Domain;

public class Entrada : Entity
{
    public Guid DiarioId { get; private set; }
    public string Conteudo { get; private set; }
    public DateTime DataDaEntrada { get; private set; }
    public DateTime DataDeCadastro { get; private set; }
    public DateTime? DataDeAlteracao { get; private set; }

    // EF Rel.
    public Diario Diario { get; private set; }

    protected Entrada() { }

    public Entrada(Guid diarioId, string conteudo)
    {
        DiarioId = diarioId;
        Conteudo = conteudo;
        DataDaEntrada = DateTime.UtcNow;
        DataDeCadastro = DateTime.UtcNow;
    }

    public void AtualizarConteudo(string conteudo)
    {
        if (string.IsNullOrWhiteSpace(conteudo))
            throw new DomainException("O conteúdo da entrada não pode estar vazio");

        Conteudo = conteudo;
        DataDeAlteracao = DateTime.UtcNow;
    }
}