using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loja.models
{
  public class Produto
  {
    public int Id { get; set; }
    public string Nome { get; set; }
    public double Preco { get; set; }
    public string Fornecedor { get; set; }
  }
}
