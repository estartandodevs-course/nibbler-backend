using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Moq;
using Nibbler.Core.Mediator;

namespace Nibbler.Diario.Infra.Data;

public class DiarioContextFactory : IDesignTimeDbContextFactory<DiarioContext>
{
    public DiarioContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DiarioContext>();
        
        optionsBuilder.UseSqlServer("Server=localhost;Database=NibblerProject;User Id=sa;Password=MinhaSenhaForte$;TrustServerCertificate=True;MultipleActiveResultSets=True;");

        // Criando um mock do IMediatorHandler para o design time
        var mediatorMock = new Mock<IMediatorHandler>();
        
        return new DiarioContext(optionsBuilder.Options, mediatorMock.Object);
    }
}