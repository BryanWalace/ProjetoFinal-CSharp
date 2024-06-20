using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Loja.data;
using Loja.models;

namespace Loja.services
{
  public class ProductService
  {
    private readonly LojaDbContext _dbContext;
    public ProductService(LojaDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    //Metodo para consultar todos os produtos
    public async Task<List<Produto>> GetAllProductsAsync()
    {
      return await _dbContext.Produtos.ToListAsync();
    }

    //Metodo para consultar um produto a partir do seu id
    public async Task<Produto> GetProductByIdAsync(int id)
    {
      return await _dbContext.Produtos.FindAsync(id);
    }

    //Metodo para gravar um novo produto
    public async Task AddProductAsync(Produto produto)
    {
      _dbContext.Produtos.Add(produto);
      await _dbContext.SaveChangesAsync();
    }

    //Metodo para tualizar os dados de um produto
    public async Task UpdateProductAsync(Produto produto)
    {
      _dbContext.Entry(produto).State = EntityState.Modified;
      await _dbContext.SaveChangesAsync();
    }

    //Metodo para excluir um produto
    public async Task DeleteProductAsync(int id)
    {
      var produto = await _dbContext.Produtos.FindAsync(id);
      if (produto != null)
      {
        _dbContext.Produtos.Remove(produto);
        await _dbContext.SaveChangesAsync();
      }
    }

    internal static async Task GetAllForncedorAsync()
    {
      throw new NotImplementedException();
    }
  }
}