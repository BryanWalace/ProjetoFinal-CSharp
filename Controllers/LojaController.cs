using Loja.models;
using Loja.services;
using Loja.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loja.Controllers
{
  [ApiController]
  [Route("api/vendas")]
  public class LojaController : ControllerBase
  {
    private readonly VendaService _vendaService;

    public LojaController(VendaService vendaService)
    {
      _vendaService = vendaService ?? throw new ArgumentNullException(nameof(vendaService));
    }

    // GET: api/vendas
    [HttpGet]
    public async Task<ActionResult<List<Venda>>> GetAllVendas()
    {
      var vendas = await _vendaService.GetAllVendasAsync();
      return Ok(vendas);
    }

    // GET: api/vendas/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Venda>> GetVendaById(int id)
    {
      var venda = await _vendaService.GetVendaByIdAsync(id);
      if (venda == null)
      {
        return NotFound();
      }
      return Ok(venda);
    }

    // POST: api/vendas
    [HttpPost]
    public async Task<IActionResult> CreateVenda([FromBody] Venda venda)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      await _vendaService.CriarVendaAsync(venda);
      return CreatedAtAction(nameof(GetVendaById), new { id = venda.Id }, venda);
    }

    // PUT: api/vendas/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVenda(int id, [FromBody] Venda venda)
    {
      if (id != venda.Id)
      {
        return BadRequest();
      }

      try
      {
        await _vendaService.UpdateVendaAsync(venda);
        return NoContent();
      }
      catch (Exception ex)
      {
        return BadRequest(new { error = ex.Message });
      }
    }

    // DELETE: api/vendas/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVenda(int id)
    {
      try
      {
        await _vendaService.DeleteVendaAsync(id);
        return NoContent();
      }
      catch (Exception ex)
      {
        return BadRequest(new { error = ex.Message });
      }
    }
  }
}
