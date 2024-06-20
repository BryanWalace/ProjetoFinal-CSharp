using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Loja.models;
using Loja.services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Loja.Services;

namespace Loja.Controllers
{
  [ApiController]
  [Route("api/vendas")]
  [Authorize] // Requer autenticação para acessar endpoints deste controlador
  public class VendaController : ControllerBase
  {
    private readonly VendaService _vendaService;
    private readonly DepositoService _depositoService;

    public VendaController(VendaService vendaService, DepositoService depositoService)
    {
      _vendaService = vendaService ?? throw new ArgumentNullException(nameof(vendaService));
      _depositoService = depositoService ?? throw new ArgumentNullException(nameof(depositoService));
    }

    [HttpPost]
    public async Task<IActionResult> CriarVenda([FromBody] Venda venda)
    {
      try
      {
        var novaVenda = await _vendaService.CriarVendaAsync(venda);
        await _depositoService.AtualizarEstoqueAposVendaAsync(venda.ProdutoId, venda.QuantidadeVendida);
        return Ok(novaVenda);
      }
      catch (Exception ex)
      {
        return BadRequest(new { error = ex.Message });
      }
    }

    [HttpGet]
    public async Task<ActionResult<List<Venda>>> ListarVendas()
    {
      var vendas = await _vendaService.GetAllVendasAsync();
      return Ok(vendas);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Venda>> ObterVendaPorId(int id)
    {
      var venda = await _vendaService.GetVendaByIdAsync(id);
      if (venda == null)
      {
        return NotFound();
      }
      return Ok(venda);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarVenda(int id, [FromBody] Venda venda)
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> ExcluirVenda(int id)
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
