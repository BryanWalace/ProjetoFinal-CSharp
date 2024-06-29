using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Loja.models;

public class Contratos
{
  [Key]
  public int Id { get; set; }

  [Required]
  public int ClienteId { get; set; }

  [ForeignKey(nameof(ClienteId))] // Definindo explicitamente a chave estrangeira
  [JsonIgnore]
  public Cliente Cliente { get; set; } // Propriedade de navegação para Cliente

  [Required]
  public int ServicoId { get; set; }

  [ForeignKey(nameof(ServicoId))] // Definindo explicitamente a chave estrangeira
  [JsonIgnore]
  public Servico Servico { get; set; } // Propriedade de navegação para Servico

  [Required]
  public decimal PrecoCobrado { get; set; }

  [Required]
  public DateTime DataContratacao { get; set; }
}
