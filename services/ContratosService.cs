using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Loja.data;
using Loja.models;

namespace Loja.services
{
  public class ContratosService
  {
    private readonly LojaDbContext _dbContext;

    public ContratosService(LojaDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public async Task<List<Contratos>> GetAllContratosAsync()
    {
      return await _dbContext.Contratos
          .Include(c => c.Cliente)
          .Include(c => c.Servico)
          .ToListAsync();
    }

    public async Task<Contratos> GetContratoByIdAsync(int id)
    {
      return await _dbContext.Contratos
          .Include(c => c.Cliente)
          .Include(c => c.Servico)
          .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Contratos> AddContratoAsync(Contratos contrato)
    {
      _dbContext.Contratos.Add(contrato);
      await _dbContext.SaveChangesAsync();
      return contrato;
    }

    public async Task UpdateContratoAsync(Contratos contrato)
    {
      _dbContext.Entry(contrato).State = EntityState.Modified;
      await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteContratoAsync(int id)
    {
      var contrato = await _dbContext.Contratos.FindAsync(id);
      if (contrato != null)
      {
        _dbContext.Contratos.Remove(contrato);
        await _dbContext.SaveChangesAsync();
      }
    }

    public async Task<List<Servico>> GetServicosByClienteIdAsync(int clienteId)
    {
      return await _dbContext.Contratos
          .Where(c => c.ClienteId == clienteId)
          .Select(c => c.Servico)
          .ToListAsync();
    }
  }
}
