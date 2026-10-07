using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DermaFer360.Dominio.Entidades;

namespace DermaFer360.Repositorio.Configuracoes;

public class ProdutoConfiguracoes : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("Produtos").HasKey(p => p.Id);

        builder.Property(p => p.Id).HasColumnName("ProdutoId");
        builder.Property(p => p.Nome).HasColumnName("Nome").IsRequired(true);
        builder.Property(p => p.Descricao).HasColumnName("Descricao").IsRequired(true);
        builder.Property(p => p.PrecoCusto).HasColumnName("PrecoCusto").IsRequired(true).HasPrecision(18, 2);
        builder.Property(p => p.PrecoVenda).HasColumnName("PrecoVenda").IsRequired(true).HasPrecision(18, 2);
        builder.Property(p => p.Ativo).HasColumnName("Ativo").IsRequired(true);
        builder.Property(p => p.Estoque).HasColumnName("Estoque").IsRequired(true);

        builder.HasOne(p => p.Usuario)
        .WithMany(u =>u.Produtos)
        .HasForeignKey(p => p.UsuarioId)
        .IsRequired();

    }
}