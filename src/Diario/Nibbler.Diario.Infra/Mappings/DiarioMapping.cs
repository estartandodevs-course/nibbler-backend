using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nibbler.Diario.Domain;

namespace Nibbler.Diario.Infra.Mappings;

public class DiarioMapping : IEntityTypeConfiguration<Domain.Diario>
{
    public void Configure(EntityTypeBuilder<Domain.Diario> builder)
    {
        builder.ToTable("Diarios");

        builder.HasKey(c => c.Id);

        // Configuração do Título
        builder.Property(c => c.Titulo)
            .HasMaxLength(100)
            .IsRequired();

        // Configuração do Conteúdo
        builder.Property(c => c.Conteudo)
            .HasMaxLength(1000)
            .IsRequired();

        // Relacionamento com Etiquetas
        builder.HasMany(c => c.Etiquetas)
            .WithMany(c => c.Diarios)
            .UsingEntity<Dictionary<string, object>>(
                "DiarioEtiqueta",
                j => j.HasOne<Etiqueta>().WithMany().HasForeignKey("EtiquetaId"),
                j => j.HasOne<Domain.Diario>().WithMany().HasForeignKey("DiarioId")
            );

        // Relacionamento com Reflexões
        builder.HasMany(c => c.Reflexoes)
            .WithOne()
            .HasForeignKey("DiarioId");
        
        // Relacionamento com Entradas
        builder.HasMany(c => c.Entradas)
            .WithOne(e => e.Diario)
            .HasForeignKey(e => e.DiarioId);


        // Ignora a propriedade DataDeExclusao no soft delete
        builder.Property(c => c.DataDeExclusao).IsRequired(false);
    }
}
