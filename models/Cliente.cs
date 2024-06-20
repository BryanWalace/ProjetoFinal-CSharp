using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loja.models
{
  public class Cliente
  {
    public int Id { get; set; }

    [Required]
    public string Nome { get; set; }

    [Required]
    public string Cpf { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    // Propriedade de navegação para as vendas deste cliente
    public ICollection<Venda> Vendas { get; set; }
  }
}
