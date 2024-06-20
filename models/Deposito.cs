using System.Collections.Generic;
using Loja.models;
public class Deposito
{
  public int Id { get; set; }
  public string Nome { get; set; }
  public ICollection<DepositoProduto> Produtos { get; set; }
}