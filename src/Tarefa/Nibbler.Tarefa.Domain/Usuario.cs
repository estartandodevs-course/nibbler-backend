namespace Nibbler.Tarefa.Domain;

public class Usuario
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    

    protected Usuario(){}

    public Usuario(Guid id, string nome, string foto)
    {
        Id = id;
        Nome = nome;
    }
}
