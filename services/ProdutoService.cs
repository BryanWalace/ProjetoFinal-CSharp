using Loja.data;
using Loja.models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loja.services
{
  public class ProductService
  {
    private readonly LojaDbContext _context;

    public ProductService(LojaDbContext context)
    {
      _context = context;
    }

    // Retorna todos os produtos de forma assíncrona.
    public async Task<List<Produto>> GetAllProductsAsync()
    {
      return await _context.Produtos.ToListAsync();
    }

    // Retorna um produto pelo seu ID de forma assíncrona.
    public async Task<Produto> GetProductByIdAsync(int id)
    {
      return await _context.Produtos.FindAsync(id);
    }

    // Adiciona um novo produto de forma assíncrona.
    public async Task AddProductAsync(Produto produto)
    {
      _context.Produtos.Add(produto);
      await _context.SaveChangesAsync();
    }

    // Atualiza um produto existente de forma assíncrona.
    public async Task UpdateProductAsync(Produto produto)
    {
      _context.Entry(produto).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    }

    // Remove um produto pelo seu ID de forma assíncrona.
    public async Task DeleteProductAsync(int id)
    {
      var produto = await _context.Produtos.FindAsync(id);
      if (produto != null)
      {
        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
      }
    }
  }
}
