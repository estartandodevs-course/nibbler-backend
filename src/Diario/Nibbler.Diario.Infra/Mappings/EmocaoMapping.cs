using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nibbler.Diario.Domain;

namespace Nibbler.Diario.Infra.Mappings;

public class EmocaoMapping : IEntityTypeConfiguration<Emocao>
{
    public void Configure(EntityTypeBuilder<Emocao> builder)
    {
        builder.ToTable("Emocoes");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Nome)
            .IsRequired();

        builder.Property(e => e.DataRegistro)
            .IsRequired();
    }
}