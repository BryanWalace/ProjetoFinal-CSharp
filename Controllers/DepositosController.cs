using Microsoft.AspNetCore.Mvc;
using Loja.models;
using Loja.services;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/depositos")]
public class DepositosController : ControllerBase
{
  private readonly DepositoService _depositoService;

  public DepositosController(DepositoService depositoService)
  {
    _depositoService = depositoService ?? throw new ArgumentNullException(nameof(depositoService));
  }

  // GET: api/depositos
  [HttpGet]
  public async Task<ActionResult<List<Deposito>>> ObterTodosDepositos()
  {
    var depositos = await _depositoService.ObterTodosDepositosAsync();
    return Ok(depositos);
  }

  // GET: api/depositos/5
  [HttpGet("{id}")]
  public async Task<ActionResult<Deposito>> ObterDepositoPorId(int id)
  {
    var deposito = await _depositoService.ObterDepositoPorIdAsync(id);
    if (deposito == null)
    {
      return NotFound();
    }
    return Ok(deposito);
  }

  // POST: api/depositos
  [HttpPost]
  public async Task<ActionResult<Deposito>> AdicionarDeposito(Deposito deposito)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    await _depositoService.AdicionarDepositoAsync(deposito);
    return CreatedAtAction(nameof(ObterDepositoPorId), new { id = deposito.Id }, deposito);
  }

  // PUT: api/depositos/5
  [HttpPut("{id}")]
  public async Task<IActionResult> AtualizarDeposito(int id, Deposito deposito)
  {
    if (id != deposito.Id)
    {
      return BadRequest();
    }

    try
    {
      await _depositoService.AtualizarDepositoAsync(deposito);
      return Ok();
    }
    catch (Exception ex)
    {
      return BadRequest(new { error = ex.Message });
    }
  }

  // DELETE: api/depositos/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> RemoverDeposito(int id)
  {
    try
    {
      await _depositoService.RemoverDepositoAsync(id);
      return Ok();
    }
    catch (Exception ex)
    {
      return BadRequest(new { error = ex.Message });
    }
  }

  // POST: api/depositos/1/produtos/5?quantidade=10
  [HttpPost("{idDeposito}/produtos/{idProduto}")]
  public async Task<IActionResult> AdicionarProdutoAoDeposito(int idDeposito, int idProduto, [FromQuery] int quantidade)
  {
    try
    {
      await _depositoService.AdicionarProdutoAoDepositoAsync(idDeposito, idProduto, quantidade);
      return Ok();
    }
    catch (Exception ex)
    {
      return BadRequest(new { error = ex.Message });
    }
  }

  // DELETE: api/depositos/1/produtos/5?quantidade=5
  [HttpDelete("{idDeposito}/produtos/{idProduto}")]
  public async Task<IActionResult> RemoverProdutoDoDeposito(int idDeposito, int idProduto, [FromQuery] int quantidade)
  {
    try
    {
      await _depositoService.RemoverProdutoDoDepositoAsync(idDeposito, idProduto, quantidade);
      return Ok();
    }
    catch (Exception ex)
    {
      return BadRequest(new { error = ex.Message });
    }
  }
}
