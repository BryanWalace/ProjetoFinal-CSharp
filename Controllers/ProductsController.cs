using Microsoft.AspNetCore.Mvc;
using Loja.models;
using Loja.services;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
  private readonly ProductService _productService;

  public ProductsController(ProductService productService)
  {
    _productService = productService ?? throw new ArgumentNullException(nameof(productService));
  }

  [HttpGet]
  public async Task<ActionResult<List<Produto>>> GetAllProducts()
  {
    var produtos = await _productService.GetAllProductsAsync();
    return Ok(produtos);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Produto>> GetProductById(int id)
  {
    var produto = await _productService.GetProductByIdAsync(id);
    if (produto == null)
    {
      return NotFound();
    }
    return Ok(produto);
  }

  [HttpPost]
  public async Task<ActionResult<Produto>> AddProduct(Produto produto)
  {
    await _productService.AddProductAsync(produto);
    return CreatedAtAction(nameof(GetProductById), new { id = produto.Id }, produto);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateProduct(int id, Produto produto)
  {
    if (id != produto.Id)
    {
      return BadRequest();
    }

    try
    {
      await _productService.UpdateProductAsync(produto);
      return Ok();
    }
    catch (Exception ex)
    {
      return BadRequest(new { error = ex.Message });
    }
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteProduct(int id)
  {
    try
    {
      await _productService.DeleteProductAsync(id);
      return Ok();
    }
    catch (Exception ex)
    {
      return BadRequest(new { error = ex.Message });
    }
  }
}
