using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Loja.data;
using Loja.models;

namespace Loja.services
{
  public class ClientesService
  {
    private readonly LojaDbContext _dbContext;
    public ClientesService(LojaDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    //Metodo para consultar todos os clientes
    public async Task<List<Cliente>> GetAllClientesAsync()
    {
      return await _dbContext.Clientes.ToListAsync();
    }

    //Metodo para consultar um cliente a partir do seu id
    public async Task<Cliente> GetClientesByIdAsync(int id)
    {
      return await _dbContext.Clientes.FindAsync(id);
    }

    //Metodo para gravar um novo cliente
    public async Task AddClientesAsync(Cliente cliente)
    {
      _dbContext.Clientes.Add(cliente);
      await _dbContext.SaveChangesAsync();
    }

    //Metodo para tualizar os dados de um cliente
    public async Task UpdateClientesAsync(Cliente cliente)
    {
      _dbContext.Entry(cliente).State = EntityState.Modified;
      await _dbContext.SaveChangesAsync();
    }

    //Metodo para excluir um cliente
    public async Task DeleteClientesAsync(int id)
    {
      var cliente = await _dbContext.Clientes.FindAsync(id);
      if (cliente != null)
      {
        _dbContext.Clientes.Remove(cliente);
        await _dbContext.SaveChangesAsync();
      }
    }
  }
}