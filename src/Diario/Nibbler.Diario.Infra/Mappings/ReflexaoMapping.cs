using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nibbler.Diario.Domain;

namespace Nibbler.Diario.Infra.Mappings;

public class ReflexaoMapping : IEntityTypeConfiguration<Reflexao>
{
    public void Configure(EntityTypeBuilder<Reflexao> builder)
    {
        builder.ToTable("Reflexoes");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Conteudo)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(r => r.DataDeCadastro)
            .IsRequired();

        // Relacionamento com Diário
        builder.HasOne<Domain.Diario>()
            .WithMany(d => d.Reflexoes)
            .HasForeignKey("DiarioId")
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        // Relacionamento com Usuário
        builder.HasOne(r => r.Usuario)
            .WithMany()
            .HasForeignKey("UsuarioId")
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}