using Loja.data;
using Loja.models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loja.services
{
  public class DepositoService
  {
    private readonly LojaDbContext _contexto;

    public DepositoService(LojaDbContext contexto)
    {
      _contexto = contexto;
    }

    // Retorna todos os depósitos.
    public async Task<List<Deposito>> ObterTodosDepositosAsync()
    {
      return await _contexto.Depositos.ToListAsync();
    }

    // Retorna um depósito pelo seu ID.
    public async Task<Deposito> ObterDepositoPorIdAsync(int id)
    {
      return await _contexto.Depositos.FindAsync(id);
    }

    // Retorna a lista de produtos e suas quantidades em um depósito específico.
    public async Task<List<ProdutoQuantidade>> GetProdutosNoDepositoAsync(int idDeposito)
    {
      return await _contexto.DepositoProdutos
          .Where(dp => dp.DepositoId == idDeposito)
          .Select(dp => new ProdutoQuantidade
          {
            NomeProduto = dp.Produto.Nome,
            Quantidade = dp.Quantidade
          })
          .ToListAsync();
    }

    // Retorna a quantidade de um produto específico em um depósito.
    public async Task<ProdutoQuantidade> GetQuantidadeProdutoAsync(int idProduto)
    {
      var depositoProduto = await _contexto.DepositoProdutos
          .Include(dp => dp.Produto)
          .FirstOrDefaultAsync(dp => dp.ProdutoId == idProduto);

      return depositoProduto != null
          ? new ProdutoQuantidade
          {
            NomeProduto = depositoProduto.Produto.Nome,
            Quantidade = depositoProduto.Quantidade
          }
          : null;
    }

    // Adiciona um novo depósito.
    public async Task AdicionarDepositoAsync(Deposito deposito)
    {
      _contexto.Depositos.Add(deposito);
      await _contexto.SaveChangesAsync();
    }

    // Atualiza um depósito existente.
    public async Task AtualizarDepositoAsync(Deposito deposito)
    {
      _contexto.Entry(deposito).State = EntityState.Modified;
      await _contexto.SaveChangesAsync();
    }

    // Remove um depósito pelo seu ID.
    public async Task RemoverDepositoAsync(int id)
    {
      var deposito = await _contexto.Depositos.FindAsync(id);
      if (deposito != null)
      {
        _contexto.Depositos.Remove(deposito);
        await _contexto.SaveChangesAsync();
      }
    }

    // Atualiza o estoque de um produto após uma venda.
    public async Task AtualizarEstoqueAposVendaAsync(int idProduto, int quantidadeVendida)
    {
      var depositoProduto = await _contexto.DepositoProdutos.FindAsync(idProduto);
      if (depositoProduto != null)
      {
        depositoProduto.Quantidade -= quantidadeVendida;
        if (depositoProduto.Quantidade <= 0)
        {
          _contexto.DepositoProdutos.Remove(depositoProduto);
        }
        else
        {
          _contexto.Entry(depositoProduto).State = EntityState.Modified;
        }
        await _contexto.SaveChangesAsync();
      }
    }

    // Adiciona um produto ao depósito ou atualiza sua quantidade se já existir.
    public async Task AdicionarProdutoAoDepositoAsync(int idDeposito, int idProduto, int quantidade)
    {
      var depositoProduto = await _contexto.DepositoProdutos.FindAsync(idDeposito, idProduto);
      if (depositoProduto == null)
      {
        depositoProduto = new DepositoProduto
        {
          DepositoId = idDeposito,
          ProdutoId = idProduto,
          Quantidade = quantidade
        };
        _contexto.DepositoProdutos.Add(depositoProduto);
      }
      else
      {
        depositoProduto.Quantidade += quantidade;
        _contexto.Entry(depositoProduto).State = EntityState.Modified;
      }
      await _contexto.SaveChangesAsync();
    }

    // Remove um produto do depósito.
    public async Task RemoverProdutoDoDepositoAsync(int idDeposito, int idProduto, int quantidade)
    {
      var depositoProduto = await _contexto.DepositoProdutos.FindAsync(idDeposito, idProduto);
      if (depositoProduto != null)
      {
        depositoProduto.Quantidade -= quantidade;
        if (depositoProduto.Quantidade <= 0)
        {
          _contexto.DepositoProdutos.Remove(depositoProduto);
        }
        else
        {
          _contexto.Entry(depositoProduto).State = EntityState.Modified;
        }
        await _contexto.SaveChangesAsync();
      }
    }

    // Consulta a lista de produtos e suas quantidades em um depósito específico.
    public async Task<List<ProdutoQuantidade>> ConsultarProdutosNoDepositoSumarizadaAsync(int idDeposito)
    {
      return await _contexto.DepositoProdutos
          .Where(dp => dp.DepositoId == idDeposito)
          .Select(dp => new ProdutoQuantidade
          {
            NomeProduto = dp.Produto.Nome,
            Quantidade = dp.Quantidade
          })
          .ToListAsync();
    }

    // Consulta a quantidade de um produto específico em um depósito.
    public async Task<ProdutoQuantidade> ConsultarQuantidadeProdutoNoDepositoAsync(int idProduto)
    {
      var depositoProduto = await _contexto.DepositoProdutos
          .Include(dp => dp.Produto)
          .FirstOrDefaultAsync(dp => dp.ProdutoId == idProduto);

      return depositoProduto != null
          ? new ProdutoQuantidade
          {
            NomeProduto = depositoProduto.Produto.Nome,
            Quantidade = depositoProduto.Quantidade
          }
          : null;
    }
  }
}
