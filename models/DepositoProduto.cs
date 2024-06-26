using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Loja.models;

public class DepositoProduto
{
  [Key]
  public int Id { get; set; }
  public int DepositoId { get; set; }
  public virtual Deposito Deposito { get; set; }
  [ForeignKey("Produto")]
  public int ProdutoId { get; set; }
  public virtual Produto Produto { get; set; }
  public int Quantidade { get; set; }
}
