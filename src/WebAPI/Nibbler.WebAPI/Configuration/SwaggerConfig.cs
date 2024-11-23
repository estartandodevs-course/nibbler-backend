using System.Reflection;
using Microsoft.OpenApi.Models;

namespace Nibbler.WebAPI.Configuration;


public static class SwaggerConfig
{
    public static void AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "Projeto Nibbler Estartando Devs 2024",
                Description = "Este projeto é uma API do estartando devs",
                Contact = new OpenApiContact() { Name = "ZézinMandaBala", Email = "FumadorDeBalaHalls@gmail.com.br" }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);

        });
    }

    public static void UseSwaggerConfiguration(this IApplicationBuilder app)
    {
        app.UseSwagger(c =>
        {
            c.RouteTemplate = "swagger/{documentname}/swagger.json";
        });

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Forum Estartando Devs 2024");
            c.RoutePrefix = "swagger";
        });
    }
}
