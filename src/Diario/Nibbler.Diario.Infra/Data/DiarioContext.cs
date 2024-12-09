using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Nibbler.Core.Data;
using Nibbler.Core.Mediator;
using Nibbler.Core.Messages;
using Nibbler.Diario.Domain;
using Nibbler.Diario.Infra.Mappings;
using Nibbler.Usuario.Infra.Data;

namespace Nibbler.Diario.Infra.Data;

public class DiarioContext : DbContext, IUnitOfWorks
{
    private readonly IMediatorHandler _mediatorHandler;
    private static readonly TimeZoneInfo cetZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

    public DiarioContext(DbContextOptions<DiarioContext> options, IMediatorHandler mediatorHandler)
        : base(options)
    {
        _mediatorHandler = mediatorHandler;
    }

    public DbSet<Domain.Diario> Diarios { get; set; }
    public DbSet<Etiqueta> Etiquetas { get; set; }
    public DbSet<Reflexao> Reflexoes { get; set; }
    public DbSet<Entrada> Entradas { get; set; }
    public DbSet<Emocao> Emocoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Event>();
        modelBuilder.Ignore<ValidationResult>();

        modelBuilder.ApplyConfiguration(new DiarioMapping());
        modelBuilder.ApplyConfiguration(new EtiquetaMapping());
        modelBuilder.ApplyConfiguration(new ReflexaoMapping());
        modelBuilder.ApplyConfiguration(new EntradaMapping());
        modelBuilder.ApplyConfiguration(new EmocaoMapping());

        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> Commit()
    {
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