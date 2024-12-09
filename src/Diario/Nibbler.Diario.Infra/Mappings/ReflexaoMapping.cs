using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nibbler.Diario.Domain;

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

        // Relacionamento com Usuário
        builder.Property(r => r.UsuarioId)
            .IsRequired();

        // Relacionamento com Emoção
        builder.HasOne(r => r.Emocao)
            .WithMany(e => e.Reflexoes)
            .HasForeignKey(r => r.EmocaoId)
            .IsRequired(false);
    }
}