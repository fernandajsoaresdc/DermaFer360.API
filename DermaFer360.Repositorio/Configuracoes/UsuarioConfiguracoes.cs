using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DermaFer360.Dominio.Entidades;

namespace DermaFer360.Repositorio.Configuracoes;

public class UsuarioConfiguracoes : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios").HasKey(u => u.Id);

        builder.Property(u => u.Id).HasColumnName("UsuarioId");
        builder.Property(u => u.Nome).HasColumnName("Nome").IsRequired(true).HasMaxLength(100);
        builder.Property(u => u.Email).HasColumnName("Email").IsRequired(true).HasMaxLength(254);
        builder.Property(u => u.Senha).HasColumnName("Senha").IsRequired(true);
        builder.Property(u => u.Ativo).HasColumnName("Ativo").IsRequired(true);
        builder.Property(u => u.Cargo).HasColumnName("Cargo").HasConversion<int>().IsRequired(true);

      

    }
}
