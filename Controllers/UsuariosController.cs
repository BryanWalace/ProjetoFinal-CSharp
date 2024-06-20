using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Loja.models;
using Loja.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
  private readonly UsuariosService _usuariosService;

  public UsuariosController(UsuariosService usuariosService)
  {
    _usuariosService = usuariosService ?? throw new ArgumentNullException(nameof(usuariosService));
  }

  [HttpGet]
  [Authorize] // Endpoint requer autorização
  public async Task<ActionResult<List<Usuario>>> GetAllUsuarios()
  {
    var usuarios = await _usuariosService.GetAllUsuariosAsync();
    return Ok(usuarios);
  }

  [HttpGet("{id}")]
  [Authorize] // Endpoint requer autorização
  public async Task<ActionResult<Usuario>> GetUsuarioById(int id)
  {
    var usuario = await _usuariosService.GetUsuarioByIdAsync(id);
    if (usuario == null)
    {
      return NotFound();
    }
    return Ok(usuario);
  }

  [HttpPost]
  [Authorize] // Endpoint requer autorização
  public async Task<ActionResult<Usuario>> AddUsuario(Usuario usuario)
  {
    await _usuariosService.AddUsuarioAsync(usuario);
    return CreatedAtAction(nameof(GetUsuarioById), new { id = usuario.Id }, usuario);
  }

  [HttpPut("{id}")]
  [Authorize] // Endpoint requer autorização
  public async Task<IActionResult> UpdateUsuario(int id, Usuario usuario)
  {
    if (id != usuario.Id)
    {
      return BadRequest();
    }

    try
    {
      await _usuariosService.UpdateUsuarioAsync(usuario);
      return Ok();
    }
    catch (Exception ex)
    {
      return BadRequest(new { error = ex.Message });
    }
  }

  [HttpDelete("{id}")]
  [Authorize] // Endpoint requer autorização
  public async Task<IActionResult> DeleteUsuario(int id)
  {
    try
    {
      await _usuariosService.DeleteUsuarioAsync(id);
      return Ok();
    }
    catch (Exception ex)
    {
      return BadRequest(new { error = ex.Message });
    }
  }
}
