using Loja.models;

public class DepositoProduto
{
  public int DepositoId { get; set; }
  public virtual Deposito Deposito { get; set; }
  public int ProdutoId { get; set; }
  public virtual Produto Produto { get; set; }
  public int Quantidade { get; set; }
}
