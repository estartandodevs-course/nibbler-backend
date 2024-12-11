using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nibbler.Autenticacao.App.Data;
using Nibbler.Diario.Infra.Data;
using Nibbler.Usuario.Infra.Data;

namespace Nibbler.WebAPI.Configuration;
    
public static class ApiConfig
{
    private const string ConexaoBancoDeDados = "NibblerConnection";
    private const string ConexaoBancoDeDadosIdentity = "IdentityConnection";
    private const string PermissoesDeOrigem = "_permissoesDeOrigem";

    public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.AddDbContext<UsuarioContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(ConexaoBancoDeDados)));
        
        services.AddDbContext<DiarioContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(ConexaoBancoDeDados)));
        
        services.AddDbContext<AutenticacaoDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(ConexaoBancoDeDadosIdentity)));

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddCors(options =>
        {
            options.AddPolicy(PermissoesDeOrigem,
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
    }

    public static void UseApiConfiguration(this WebApplication app)
    {
        app.UseSwaggerConfiguration();
        app.UseSwagger();
        app.UseSwaggerUI();
    
        using var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope();

        var contextUsuario = serviceScope.ServiceProvider.GetRequiredService<UsuarioContext>();
        contextUsuario.Database.Migrate();

        var contextDiario = serviceScope.ServiceProvider.GetRequiredService<DiarioContext>();
        contextDiario.Database.Migrate();

        var contextAutenticacao = serviceScope.ServiceProvider.GetRequiredService<AutenticacaoDbContext>();
        contextAutenticacao.Database.Migrate();
        
        app.MapControllers();
        
        app.UseHttpsRedirection();
    }
}
