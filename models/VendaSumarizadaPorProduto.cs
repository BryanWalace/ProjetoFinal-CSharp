namespace Loja.models
{
  public class VendaSumarizadaPorProduto
  {
    public string ProdutoNome { get; set; }
    public int TotalQuantidadeVendida { get; set; }
    public decimal TotalValorVendido { get; set; }
  }
}