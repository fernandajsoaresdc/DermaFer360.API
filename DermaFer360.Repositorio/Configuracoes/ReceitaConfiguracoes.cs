using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DermaFer360.Dominio.Entidades;

namespace DermaFer360.Repositorio.Configuracoes;

public class ReceitaConfiguracoes : IEntityTypeConfiguration<Receita>
{
    public void Configure(EntityTypeBuilder<Receita> builder)
    {
        builder.ToTable("Receitas").HasKey(r => r.Id);

        builder.Property(r => r.Id).HasColumnName("ReceitaId");
         builder.Property(r => r.Quantidade).HasColumnName("Quantidade").IsRequired(true);
        builder.Property(r => r.PrecoUnitario).HasColumnName("PrecoUnitario").HasColumnType("decimal(18,2)").IsRequired(true);
        builder.Property(r => r.DataReceita).HasColumnName("DataReceita").IsRequired(true);
        builder.Property(r => r.Ativo).HasColumnName("Ativo").IsRequired(true);

        builder.HasOne(r => r.Produto)
        .WithMany()
        .HasForeignKey(r => r.ProdutoId)
        .OnDelete(DeleteBehavior.Restrict)
        .IsRequired();

        builder.HasOne(r => r.Usuario)
        .WithMany()
        .HasForeignKey(r => r.UsuarioId)
        .OnDelete(DeleteBehavior.Restrict)
        .IsRequired();
    }
}
      