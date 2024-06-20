using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Loja.data;
using Loja.models;

namespace Loja.Services
{
  public class UsuariosService
  {
    private readonly LojaDbContext _dbContext;

    public UsuariosService(LojaDbContext dbContext)
    {
      _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    // Retorna todos os usuários de forma assíncrona.
    public async Task<List<Usuario>> GetAllUsuariosAsync()
    {
      return await _dbContext.Usuarios.ToListAsync();
    }

    // Retorna um usuário pelo seu ID de forma assíncrona.
    public async Task<Usuario> GetUsuarioByIdAsync(int id)
    {
      return await _dbContext.Usuarios.FindAsync(id);
    }

    // Adiciona um novo usuário de forma assíncrona.
    public async Task AddUsuarioAsync(Usuario usuario)
    {
      if (usuario == null)
      {
        throw new ArgumentNullException(nameof(usuario), "Usuário não pode ser nulo.");
      }

      _dbContext.Usuarios.Add(usuario);
      await _dbContext.SaveChangesAsync();
    }

    // Atualiza um usuário existente de forma assíncrona.
    public async Task UpdateUsuarioAsync(Usuario usuario)
    {
      if (usuario == null)
      {
        throw new ArgumentNullException(nameof(usuario), "Usuário não pode ser nulo.");
      }

      _dbContext.Entry(usuario).State = EntityState.Modified;
      await _dbContext.SaveChangesAsync();
    }

    // Retorna um usuário pelo seu email de forma assíncrona.
    public async Task<Usuario> GetUsuarioByEmailAsync(string email)
    {
      if (string.IsNullOrWhiteSpace(email))
      {
        throw new ArgumentException("O email não pode ser nulo ou vazio.", nameof(email));
      }

      return await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
    }

    // Remove um usuário pelo seu ID de forma assíncrona.
    public async Task DeleteUsuarioAsync(int id)
    {
      var usuario = await _dbContext.Usuarios.FindAsync(id);
      if (usuario != null)
      {
        _dbContext.Usuarios.Remove(usuario);
        await _dbContext.SaveChangesAsync();
      }
    }
  }
}
