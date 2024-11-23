using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nibbler.Diario.Domain;

namespace Nibbler.Diario.Infra.Mappings;

public class EtiquetaMapping : IEntityTypeConfiguration<Etiqueta>
{
    public void Configure(EntityTypeBuilder<Etiqueta> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Nome)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder.Property(e => e.DataDeCadastro)
            .IsRequired();

        builder.ToTable("Etiquetas");
    }
}