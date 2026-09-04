using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DermaFer360.Dominio.Entidades;

namespace DermaFer360.Repositorio.Configuracoes;

public class DespesaConfiguracoes : IEntityTypeConfiguration<Despesa>
{
    public void Configure(EntityTypeBuilder<Despesa> builder)
    {
        builder.ToTable("Despesas").HasKey(d => d.Id);

        builder.Property(d => d.Id).HasColumnName("DespesaId");
        builder.Property(d => d.Descricao).HasColumnName("Descricao").IsRequired(true);
        builder.Property(d => d.Valor).HasColumnName("Valor").HasColumnType("decimal(18,2)").IsRequired(true);
        builder.Property(d => d.DataDespesa).HasColumnName("DataDespesa").IsRequired(true);
        builder.Property(d => d.Ativo).HasColumnName("Ativo").IsRequired(true);


        builder.HasOne(d => d.Usuario)
        .WithMany()
        .HasForeignKey(d => d.UsuarioId)
        .OnDelete(DeleteBehavior.Restrict)
        .IsRequired();

         builder.HasOne(d => d.Produto)
        .WithMany()
        .HasForeignKey(d => d.ProdutoId)
        .OnDelete(DeleteBehavior.Restrict)
        .IsRequired(false);

    }
}