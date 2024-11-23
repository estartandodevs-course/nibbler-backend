using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nibbler.Diario.Domain;

namespace Nibbler.Diario.Infra.Mappings;

public class EntradaMapping : IEntityTypeConfiguration<Entrada>
{
    public void Configure(EntityTypeBuilder<Entrada> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Conteudo)
            .IsRequired()
            .HasColumnType("varchar(5000)");

        builder.Property(e => e.DataDaEntrada)
            .IsRequired();

        builder.Property(e => e.DataDeCadastro)
            .IsRequired();
        
        builder.Property(e => e.DataDeAlteracao);
        
        builder.HasOne(e => e.Diario)
            .WithMany(d => d.Entradas)
            .HasForeignKey(e => e.DiarioId);

        builder.ToTable("Entradas");
    }
}