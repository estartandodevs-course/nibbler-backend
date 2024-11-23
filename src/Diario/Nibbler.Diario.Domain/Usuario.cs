namespace Nibbler.Diario.Domain;

public class Usuario
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string CaminhoFoto { get; private set; }

    protected Usuario(){}

    public Usuario(Guid id, string nome, string caminhoFoto)
    {
        Id = id;
        Nome = nome;
        CaminhoFoto = caminhoFoto;
    }
}
