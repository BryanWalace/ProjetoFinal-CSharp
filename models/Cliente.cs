using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; // Import this namespace for JsonPropertyName attribute

namespace Loja.models
{
  public class Cliente
  {
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo CPF é obrigatório.")]
    public string Cpf { get; set; }

    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O campo Email deve ser um endereço de email válido.")]
    public string Email { get; set; }

    // Navigation properties
    public ICollection<Venda> Vendas { get; set; }

    public ICollection<Contratos> contratos { get; set; }
  }
}
