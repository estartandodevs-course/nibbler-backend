using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nibbler.Diario.Infra.Data;
using Nibbler.Usuario.Infra.Data;

namespace Nibbler.WebAPI.Configuration;
    
public static class ApiConfig
{
    private const string ConexaoBancoDeDados = "NibblerConnection";
    private const string PermissoesDeOrigem = "_permissoesDeOrigem";

    public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.AddDbContext<UsuarioContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(ConexaoBancoDeDados)));
        
        services.AddDbContext<DiarioContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(ConexaoBancoDeDados)));

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
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerConfiguration();
            app.UseSwagger();
            app.UseSwaggerUI();

        }

        app.MapControllers();
        
        app.UseHttpsRedirection();
    }
}
