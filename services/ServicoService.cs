using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Loja.data;
using Loja.models;

namespace Loja.services
{
  public class ServicoService
  {
    private readonly LojaDbContext _context;

    public ServicoService(LojaDbContext context)
    {
      _context = context;
    }

    public async Task<List<Servico>> GetAllServicosAsync()
    {
      return await _context.Servicos.ToListAsync();
    }

    public async Task<Servico> GetServicoByIdAsync(int id)
    {
      return await _context.Servicos.FindAsync(id);
    }

    public async Task AddServicoAsync(Servico servico)
    {
      _context.Servicos.Add(servico);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateServicoAsync(Servico servico)
    {
      _context.Entry(servico).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    }

    public async Task DeleteServicoAsync(int id)
    {
      var servico = await _context.Servicos.FindAsync(id);
      if (servico != null)
      {
        _context.Servicos.Remove(servico);
        await _context.SaveChangesAsync();
      }
    }
  }
}
