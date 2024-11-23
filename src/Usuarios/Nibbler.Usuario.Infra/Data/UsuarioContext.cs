using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Nibbler.Core.Data;
using Nibbler.Core.DomainObjects;
using Nibbler.Core.Mediator;
using Nibbler.Core.Messages;
using Nibbler.Core.Utilities;

namespace Nibbler.Usuario.Infra.Data;
public class UsuarioContext : DbContext, IUnitOfWorks
{
    private readonly IMediatorHandler _mediatorHandler;

    public DbSet<Domain.Usuario> Usuarios { get; set; }

    public UsuarioContext(DbContextOptions<UsuarioContext> options, IMediatorHandler mediatorHandler)
        : base(options)
    {
        _mediatorHandler = mediatorHandler;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Ignorar classes desnecessárias no mapeamento
        modelBuilder.Ignore<ValidationResult>();
        modelBuilder.Ignore<Event>();

        // Aplicar a configuração de mapeamento da entidade Usuario
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsuarioContext).Assembly);

        // Mapeamento explícito do nome da tabela 'Usuarios'
        modelBuilder.Entity<Domain.Usuario>(entity =>
        {
            entity.ToTable("Usuarios"); // Define o nome da tabela no banco de dados
            entity.HasKey(e => e.Id);   // Configura a chave primária
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100); // Exemplo de propriedade
        });
    }

    public async Task<bool> Commit()
    {
        var cetZone = ZonaDeTempo.ObterZonaDeTempo();

        foreach (var entry in ChangeTracker.Entries()
                    .Where(entry => entry.Entity.GetType().GetProperty("DataDeCadastro") != null))
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("DataDeCadastro").CurrentValue =
                    TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cetZone);
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property("DataDeCadastro").IsModified = false;
                entry.Property("DataDeAlteracao").CurrentValue =
                    TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cetZone);
            }
        }

        var sucesso = await SaveChangesAsync() > 0;

        if (sucesso) await _mediatorHandler.PublicarEventos(this);

        return sucesso;
    }
}

public static class MediatorExtension
{
    public static async Task PublicarEventos<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.Notificacoes)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.LimparEventos());

        var tasks = domainEvents.Select(async (domainEvent) => { await mediator.PublicarEvento(domainEvent); });

        await Task.WhenAll(tasks);
    }
}
