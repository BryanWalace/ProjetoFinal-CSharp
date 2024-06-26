using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Loja.models;
using Loja.services;

namespace Loja.controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ContratosController : ControllerBase
  {
    private readonly ContratosService _contratosService;

    public ContratosController(ContratosService contratosService)
    {
      _contratosService = contratosService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateContrato([FromBody] Contratos contrato)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      await _contratosService.AddContratoAsync(contrato);
      return CreatedAtAction(nameof(GetContratoById), new { id = contrato.Id }, contrato);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetContratoById(int id)
    {
      var contrato = await _contratosService.GetContratoByIdAsync(id);
      if (contrato == null)
      {
        return NotFound();
      }
      return Ok(contrato);
    }
  }
}
