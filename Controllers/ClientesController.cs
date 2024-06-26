using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Loja.models;
using Loja.services;

namespace Loja.controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ClientesController : ControllerBase
  {
    private readonly ClientesService _clientesService;
    private readonly ContratosService _contratosService;

    public ClientesController(ClientesService clientesService, ContratosService contratosService)
    {
      _clientesService = clientesService;
      _contratosService = contratosService;
    }

    [HttpGet("{clienteId}/servicos")]
    [Authorize]
    public async Task<IActionResult> GetServicosPorClienteId(int clienteId)
    {
      var cliente = await _clientesService.GetClienteByIdAsync(clienteId);
      if (cliente == null)
      {
        return NotFound("Cliente não encontrado.");
      }

      var servicos = await _contratosService.GetServicosByClienteIdAsync(clienteId);
      return Ok(servicos);
    }
  }
}
