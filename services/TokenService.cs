using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Loja.Controllers
{
  public class TokenService
  {
    private readonly string _secretKey = "qwertyuiopasdfghjklzxcvbnmqwerty";

    // Gera um token JWT com base no email fornecido.
    // Retorna o token JWT gerado.
    public string GenerateToken(string email)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_secretKey);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[] { new Claim("email", email) }),
        Expires = DateTime.UtcNow.AddHours(1), // Token expira em 1 hora
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };

      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }

    // Valida um token JWT.
    // Retorna true se o token for válido, caso contrário false.
    public bool ValidateToken(string token)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_secretKey);
      var validationParameters = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
      };

      try
      {
        // Verifica e valida o token
        tokenHandler.ValidateToken(token, validationParameters, out _);
        return true;
      }
      catch
      {
        // Caso o token seja inválido
        return false;
      }
    }
  }
}
