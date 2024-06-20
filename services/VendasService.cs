using Loja.data;
using Loja.models;
using Loja.services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loja.Services
{
  // Serviço para operações relacionadas a vendas.
  public class VendaService
  {
    private readonly LojaDbContext _context;
    private readonly DepositoService _depositoService;

    public VendaService(LojaDbContext context, DepositoService depositoService)
    {
      _context = context;
      _depositoService = depositoService;
    }

    // Retorna todas as vendas de forma assíncrona.
    public async Task<List<Venda>> GetAllVendasAsync()
    {
      return await _context.Vendas
          .Include(v => v.Cliente)
          .Include(v => v.Produto)
          .ToListAsync();
    }

    // Retorna uma venda pelo seu ID de forma assíncrona.
    public async Task<Venda> GetVendaByIdAsync(int id)
    {
      return await _context.Vendas
          .Include(v => v.Cliente)
          .Include(v => v.Produto)
          .FirstOrDefaultAsync(v => v.Id == id);
    }

    // Cria uma nova venda de forma assíncrona.
    public async Task<Venda> CriarVendaAsync(Venda venda)
    {
      // Verificar se o cliente existe
      var cliente = await _context.Clientes.FindAsync(venda.ClienteId);
      if (cliente == null)
      {
        throw new ArgumentException("Cliente não encontrado.");
      }

      // Verificar se o produto existe
      var produto = await _context.Produtos.FindAsync(venda.ProdutoId);
      if (produto == null)
      {
        throw new ArgumentException("Produto não encontrado.");
      }

      // Calcular o PrecoVenda
      venda.PrecoVenda = venda.PrecoUnitario * venda.QuantidadeVendida;

      _context.Vendas.Add(venda);
      await _context.SaveChangesAsync();

      // Atualizar estoque após a venda
      await _depositoService.AtualizarEstoqueAposVendaAsync(venda.ProdutoId, venda.QuantidadeVendida);

      return venda;
    }

    // Atualiza uma venda existente de forma assíncrona.
    public async Task UpdateVendaAsync(Venda venda)
    {
      // Recalcular o PrecoVenda
      venda.PrecoVenda = venda.PrecoUnitario * venda.QuantidadeVendida;

      _context.Entry(venda).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    }

    // Remove uma venda pelo seu ID de forma assíncrona.
    public async Task DeleteVendaAsync(int id)
    {
      var venda = await _context.Vendas.FindAsync(id);
      if (venda != null)
      {
        _context.Vendas.Remove(venda);
        await _context.SaveChangesAsync();
      }
    }

    // Consulta as vendas detalhadas por produto de forma assíncrona.
    public async Task<List<VendaDetalhadaPorProduto>> GetVendasDetalhadasPorProdutoAsync(int produtoId)
    {
      return await _context.Vendas
          .Where(v => v.ProdutoId == produtoId)
          .Select(v => new VendaDetalhadaPorProduto
          {
            ProdutoNome = v.Produto.Nome,
            DataVenda = v.DataVenda,
            VendaId = v.Id,
            ClienteNome = v.Cliente.Nome,
            QuantidadeVendida = v.QuantidadeVendida,
            PrecoVenda = v.PrecoVenda
          })
          .ToListAsync();
    }

    // Consulta as vendas sumarizadas por produto de forma assíncrona.
    public async Task<VendaSumarizadaPorProduto> GetVendasSumarizadasPorProdutoAsync(int produtoId)
    {
      return await _context.Vendas
          .Where(v => v.ProdutoId == produtoId)
          .GroupBy(v => v.ProdutoId)
          .Select(g => new VendaSumarizadaPorProduto
          {
            ProdutoNome = g.FirstOrDefault().Produto.Nome,
            TotalQuantidadeVendida = g.Sum(v => v.QuantidadeVendida),
            TotalValorVendido = g.Sum(v => v.QuantidadeVendida * v.PrecoVenda)
          })
          .FirstOrDefaultAsync();
    }

    // Consulta as vendas detalhadas por cliente de forma assíncrona.
    public async Task<List<VendaDetalhadaPorCliente>> GetVendasDetalhadasPorClienteAsync(int clienteId)
    {
      return await _context.Vendas
          .Where(v => v.ClienteId == clienteId)
          .Select(v => new VendaDetalhadaPorCliente
          {
            ProdutoNome = v.Produto.Nome,
            DataVenda = v.DataVenda,
            VendaId = v.Id,
            QuantidadeVendida = v.QuantidadeVendida,
            PrecoVenda = v.PrecoVenda
          })
          .ToListAsync();
    }

    // Consulta as vendas sumarizadas por cliente de forma assíncrona.
    public async Task<VendaSumarizadaPorCliente> GetVendasSumarizadasPorClienteAsync(int clienteId)
    {
      return await _context.Vendas
          .Where(v => v.ClienteId == clienteId)
          .GroupBy(v => v.ClienteId)
          .Select(g => new VendaSumarizadaPorCliente
          {
            TotalValorVendido = g.Sum(v => v.QuantidadeVendida * v.PrecoVenda),
            ProdutosVendidos = g.Select(v => new ProdutoQuantidade
            {
              NomeProduto = v.Produto.Nome,
              Quantidade = v.QuantidadeVendida
            }).ToList()
          })
          .FirstOrDefaultAsync();
    }

    // Verifica se um cliente existe de forma assíncrona.
    public async Task<bool> ClienteExistsAsync(int clienteId)
    {
      return await _context.Clientes.AnyAsync(c => c.Id == clienteId);
    }

    // Verifica se um produto existe de forma assíncrona.
    public async Task<bool> ProdutoExistsAsync(int produtoId)
    {
      return await _context.Produtos.AnyAsync(p => p.Id == produtoId);
    }
  }
}
