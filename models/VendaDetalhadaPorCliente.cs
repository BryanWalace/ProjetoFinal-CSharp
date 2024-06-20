namespace Loja.models
{
  public class VendaDetalhadaPorCliente
  {
    public string ProdutoNome { get; set; }
    public DateTime DataVenda { get; set; }
    public int VendaId { get; set; }
    public int QuantidadeVendida { get; set; }
    public decimal PrecoVenda { get; set; }
  }
}