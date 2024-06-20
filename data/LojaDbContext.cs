using Loja.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Loja.data
{
  public class LojaDbContext : DbContext
  {
    public LojaDbContext(DbContextOptions<LojaDbContext> options) : base(options) { }

    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Fornecedor> Fornecedores { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Venda> Vendas { get; set; }
    public DbSet<VendaItem> VendaItens { get; set; }
    public DbSet<Deposito> Depositos { get; set; }
    public DbSet<DepositoProduto> DepositoProdutos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // Configuração da entidade Produto
      modelBuilder.Entity<Produto>(entity =>
      {
        entity.HasKey(p => p.Id);
        entity.Property(p => p.Nome).IsRequired();
        entity.Property(p => p.Preco).IsRequired();
        entity.Property(p => p.Fornecedor).IsRequired();
        entity.Property(p => p.Estoque).IsRequired();
      });

      // Configuração da entidade Cliente
      modelBuilder.Entity<Cliente>(entity =>
      {
        entity.HasKey(c => c.Id);
        entity.Property(c => c.Nome).IsRequired();
        entity.Property(c => c.Cpf).IsRequired();
        entity.Property(c => c.Email).IsRequired();
      });

      // Configuração da entidade Fornecedor
      modelBuilder.Entity<Fornecedor>(entity =>
      {
        entity.HasKey(f => f.Id);
        entity.Property(f => f.Cnpj).IsRequired();
        entity.Property(f => f.Nome).IsRequired();
        entity.Property(f => f.Endereco).IsRequired();
        entity.Property(f => f.Email).IsRequired();
        entity.Property(f => f.Telefone).IsRequired();
      });

      // Configuração da entidade Usuario
      modelBuilder.Entity<Usuario>(entity =>
      {
        entity.HasKey(u => u.Id);
        entity.Property(u => u.Nome).IsRequired();
        entity.Property(u => u.Email).IsRequired();
        entity.Property(u => u.Senha).IsRequired();
      });

      // Configuração da entidade Venda
      modelBuilder.Entity<Venda>(entity =>
      {
        entity.HasKey(v => v.Id);
        entity.Property(v => v.DataVenda).IsRequired();
        entity.Property(v => v.NumeroNotaFiscal).IsRequired();
        entity.Property(v => v.QuantidadeVendida).IsRequired();
        entity.Property(v => v.PrecoUnitario).IsRequired();
        entity.Property(v => v.PrecoVenda).IsRequired();

        // Relacionamento com Cliente
        entity.HasOne(v => v.Cliente)
                  .WithMany(c => c.Vendas)
                  .HasForeignKey(v => v.ClienteId)
                  .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento com Produto
        entity.HasOne(v => v.Produto)
                  .WithMany()
                  .HasForeignKey(v => v.ProdutoId)
                  .OnDelete(DeleteBehavior.Restrict);
      });

      // Configuração da entidade VendaItem
      modelBuilder.Entity<VendaItem>(entity =>
      {
        entity.HasKey(vi => vi.Id);
        entity.Property(vi => vi.VendaId).IsRequired();
        entity.Property(vi => vi.ProdutoId).IsRequired();
        entity.Property(vi => vi.Quantidade).IsRequired();
        entity.Property(vi => vi.PrecoUnitario).IsRequired();

        // Relacionamento com Venda
        entity.HasOne(vi => vi.Venda)
                  .WithMany(v => v.Itens)
                  .HasForeignKey(vi => vi.VendaId)
                  .OnDelete(DeleteBehavior.Cascade);

        // Relacionamento com Produto
        entity.HasOne(vi => vi.Produto)
                  .WithMany()
                  .HasForeignKey(vi => vi.ProdutoId)
                  .OnDelete(DeleteBehavior.Restrict);
      });

      // Configuração da entidade Deposito
      modelBuilder.Entity<Deposito>(entity =>
      {
        entity.HasKey(d => d.Id);
        entity.Property(d => d.Nome).IsRequired();
      });

      // Configuração da entidade DepositoProduto
      modelBuilder.Entity<DepositoProduto>(entity =>
      {
        entity.HasKey(dp => new { dp.DepositoId, dp.ProdutoId });
        entity.Property(dp => dp.Quantidade).IsRequired();

        // Relacionamento com Deposito
        entity.HasOne(dp => dp.Deposito)
                  .WithMany(d => d.Produtos)
                  .HasForeignKey(dp => dp.DepositoId)
                  .OnDelete(DeleteBehavior.Cascade);

        // Relacionamento com Produto
        entity.HasOne(dp => dp.Produto)
                  .WithMany()
                  .HasForeignKey(dp => dp.ProdutoId)
                  .OnDelete(DeleteBehavior.Cascade);
      });

      base.OnModelCreating(modelBuilder);
    }
  }
}
