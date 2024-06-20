using Loja.models;
using Microsoft.EntityFrameworkCore;

namespace Loja.data
{
  public class LojaDbContext : DbContext
  {
    public LojaDbContext(DbContextOptions<LojaDbContext> options) : base(options) { }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Fornecedor> Fornecedores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Produto>().HasKey(p => p.Id);
      modelBuilder.Entity<Cliente>().HasKey(c => c.Id);
      modelBuilder.Entity<Fornecedor>().HasKey(f => f.Id);

      base.OnModelCreating(modelBuilder);
    }
  }
}
