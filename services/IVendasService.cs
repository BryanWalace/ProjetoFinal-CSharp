using Loja.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loja.services
{
  public interface IVendasService
  {
    Task<List<Venda>> GetAllVendasAsync();
    Task<Venda> GetVendaByIdAsync(int id);
    Task<Venda> CriarVendaAsync(Venda venda);
    Task UpdateVendaAsync(Venda venda);
    Task DeleteVendaAsync(int id);
    Task<List<VendaDetalhadaPorProduto>> ConsultarVendasPorProdutoDetalhadaAsync(int produtoId);
    Task<VendaSumarizadaPorProduto> ConsultarVendasPorProdutoSumarizadaAsync(int produtoId);
    Task<List<VendaDetalhadaPorCliente>> ConsultarVendasPorClienteDetalhadaAsync(int clienteId);
    Task<VendaSumarizadaPorCliente> ConsultarVendasPorClienteSumarizadaAsync(int clienteId);
    Task<bool> ClienteExistsAsync(int clienteId);
    Task<bool> ProdutoExistsAsync(int produtoId);
  }
}
