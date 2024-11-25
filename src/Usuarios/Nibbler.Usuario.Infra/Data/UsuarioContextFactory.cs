using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Moq;
using Nibbler.Core.Mediator;

namespace Nibbler.Usuario.Infra.Data;


public class UsuarioContextFactory : IDesignTimeDbContextFactory<UsuarioContext>
{
    public UsuarioContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<UsuarioContext>();
        
        optionsBuilder.UseSqlServer("Server=localhost;Database=NibblerProject;User Id=sa;Password=MinhaSenhaForte$;TrustServerCertificate=True;MultipleActiveResultSets=True;");

        // Criando um mock do IMediatorHandler para o design time
        var mediatorMock = new Mock<IMediatorHandler>();
        
        return new UsuarioContext(optionsBuilder.Options, mediatorMock.Object);
    }
}