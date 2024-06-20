using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loja.models
{
  public class Cliente
  {
    public int Id { get; set; }
    public String Nome { get; set; }
    public String Cpf { get; set; }
    public String Email { get; set; }
  }
}