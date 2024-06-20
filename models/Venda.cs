using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Loja.models
{
  public class Venda
  {
    public int Id { get; set; }

    [Required]
    public DateTime DataVenda { get; set; }

    [Required]
    public string NumeroNotaFiscal { get; set; }

    [Required]
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } // Propriedade de navegação para Cliente

    [Required]
    public int ProdutoId { get; set; }
    public Produto Produto { get; set; } // Propriedade de navegação para Produto

    [Required]
    public int QuantidadeVendida { get; set; }

    [Required]
    public decimal PrecoUnitario { get; set; }

    private decimal _precoVenda; // Campo de suporte para PrecoVenda

    // Propriedade calculada para o preço total da venda
    public decimal PrecoVenda
    {
      get => _precoVenda;
      set => _precoVenda = value; // Setter necessário para o Entity Framework Core
    }

    // Propriedade de navegação para os itens desta venda
    public ICollection<VendaItem> Itens { get; set; }
  }
}
