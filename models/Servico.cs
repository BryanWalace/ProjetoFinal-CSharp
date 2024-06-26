using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Loja.models
{
  public class Servico
  {
    [Key] // Especifica que a propriedade Id é a chave primária
    public int Id { get; set; }

    [Required] // Especifica que o campo Nome é obrigatório
    public string Nome { get; set; }

    [Required] // Especifica que o campo Preco é obrigatório
    [Column(TypeName = "decimal(18,2)")] // Define o tipo de coluna no banco de dados
    public decimal Preco { get; set; }

    public bool Status { get; set; }

    // Propriedade de navegação para Contratos
    public ICollection<Contratos> Contratos { get; set; }
  }
}
