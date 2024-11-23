using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Nibbler.Usuario.Infra.Mappings;

public class UsuarioMappings : IEntityTypeConfiguration<Domain.Usuario>
{
    public void Configure(EntityTypeBuilder<Domain.Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(x => x.Id);

        builder.Property(c => c.Nome).HasColumnName("Nome").HasMaxLength(200).IsRequired();

        builder.Property(c => c.Foto).HasColumnName("CaminhoFoto").HasMaxLength(200).IsRequired();

        builder.Property(c => c.DataDeCadastro).HasColumnName("DataDeCadastro");


        builder.OwnsOne(c => c.Login, login =>
        {
            login.Property(c => c.Hash).HasColumnName("LoginHash");

            login.OwnsOne(c => c.Email,
                email => { email.Property(x => x.Endereco).HasMaxLength(256).HasColumnName("Email"); });

            login.OwnsOne(c => c.Senha,
                senha => { senha.Property(x => x.Valor).HasMaxLength(1000).HasColumnName("Senha"); });
        });
    }
}