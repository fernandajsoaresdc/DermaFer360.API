using Microsoft.EntityFrameworkCore;
using DermaFer360.Dominio.Entidades;
using DermaFer360.Repositorio.Configuracoes;

public class DermaFer360Context : DbContext
{
    private readonly DbContextOptions _options;

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Despesa> Despesas { get; set; }
    public DbSet<Receita> Receitas { get; set; }

    public DermaFer360Context() { }

    public DermaFer360Context(DbContextOptions options) : base(options)
    {
        _options = options;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_options == null)
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-58UEHOH\SQLEXPRESS;Database=DermaFer360;Trusted_Connection=True;TrustServerCertificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UsuarioConfiguracoes());
        modelBuilder.ApplyConfiguration(new ProdutoConfiguracoes());
        modelBuilder.ApplyConfiguration(new DespesaConfiguracoes());
        modelBuilder.ApplyConfiguration(new ReceitaConfiguracoes());
    }
}