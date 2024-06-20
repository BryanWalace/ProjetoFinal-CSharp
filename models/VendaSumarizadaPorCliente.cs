namespace Loja.models
{
  public class VendaSumarizadaPorCliente
  {
    public decimal TotalValorVendido { get; set; }
    public List<ProdutoQuantidade> ProdutosVendidos { get; set; }
  }
}