using Nibbler.Core.DomainObjects;

namespace Nibbler.Usuario.Domain;

public class Usuario : Entity, IAggregateRoot
{
    public string Nome { get; private set; }
    public DateTime DataDeNascimento { get; private set; }
    public string Foto { get; private set; }
    public Login Login { get; private set; }
    

    public int Idade
    {
        get
        {
            if (DataDeNascimento == DateTime.MinValue) return 0;

            var idade = DateTime.Now.Year - DataDeNascimento.Year;

            if (DateTime.Now.Month < DataDeNascimento.Month ||
                (DateTime.Now.Month == DataDeNascimento.Month && DateTime.Now.Day < DataDeNascimento.Day))
            {
                idade--;
            }

            return idade;
        }
    }


    public Usuario(string nome,string foto, DateTime dataDeNascimento)
    {
        Nome = nome;
        Foto = foto;
        DataDeNascimento = dataDeNascimento;
    }

    public void AtribuirNome(string nome) => Nome = nome;

    public void AtribuirLogin(Login login) => Login = login;

    public void AtribuirDataDeNascimento(DateTime dataNascimento) => DataDeNascimento = dataNascimento;

    public void AtribuirFoto(string foto) => Foto = foto;
}