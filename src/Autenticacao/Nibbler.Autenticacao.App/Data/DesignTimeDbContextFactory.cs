using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Nibbler.Autenticacao.App.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AutenticacaoDbContext>
{
    public AutenticacaoDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AutenticacaoDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=NibblerIdentityDB;User Id=sa;Password=MinhaSenhaForte$;TrustServerCertificate=True;MultipleActiveResultSets=True;");

        return new AutenticacaoDbContext(optionsBuilder.Options);
    }
}
