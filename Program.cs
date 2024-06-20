using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Loja.data;
using Loja.models;
using System;
using Loja.services;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner.
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<FornecedorService>();

// Adicionar serviços ao contêiner.
// Saiba mais sobre como configurar o Swagger/OpenAPI em https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar a conexão com o banco de dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LojaDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 26))));

// Configurar o servidor Kestrel para usar HTTPS
builder.WebHost.ConfigureKestrel(options =>
{
  options.ConfigureHttpsDefaults(httpsOptions =>
  {
    httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls13;
  });
});

var app = builder.Build();

// Configurar o pipeline de solicitação HTTP.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Utilizando o Service

app.MapGet("/produtos", async (ProductService productService) =>
{
  var produtos = await productService.GetAllProductsAsync();
  return Results.Ok(produtos);
});

app.MapGet("/produtos/{id}", async (int id, ProductService productService) =>
{
  var produto = await productService.GetProductByIdAsync(id);
  if (produto == null)
  {
    return Results.NotFound($"Produto with ID {id} not found.");
  }

  return Results.Ok(produto);
});

app.MapPost("/produtos", async (Produto produto, ProductService productService) =>
{
  await productService.AddProductAsync(produto);
  return Results.Created($"/produtos/{produto.Id}", produto);
});

app.MapPut("/produtos/{id}", async (int id, Produto produto, ProductService productService) =>
{
  if (id != produto.Id)
  {
    return Results.BadRequest("Product ID mismatch.");
  }

  await productService.UpdateProductAsync(produto);
  return Results.Ok();
});

app.MapDelete("/produtos/{id}", async (int id, ProductService productService) =>
{
  await productService.DeleteProductAsync(id);
  return Results.Ok();
});


app.MapGet("/fornecedores", async (FornecedorService fornecedorService) =>
{
  var fornecedor = await fornecedorService.GetAllFornecedoresAsync();
  return Results.Ok(fornecedor);
});

app.MapGet("/fornecedores/{id}", async (int id, FornecedorService fornecedorService) =>
{
  var fornecedor = await fornecedorService.GetFornecedorByIdAsync(id);
  if (fornecedor == null)
  {
    return Results.NotFound($"Fornecedor with ID {id} not found.");
  }

  return Results.Ok(fornecedor);
});

app.MapPost("/fornecedores", async (Fornecedor fornecedor, FornecedorService fornecedorService) =>
{
  await fornecedorService.AddFornecedorAsync(fornecedor);
  return Results.Created($"/fornecedores/{fornecedor.Id}", fornecedor);
});

app.MapPut("/fornecedores/{id}", async (int id, Fornecedor fornecedor, FornecedorService fornecedorService) =>
{
  if (id != fornecedor.Id)
  {
    return Results.BadRequest("Product ID mismatch.");
  }

  await fornecedorService.UpdateFornecedorAsync(fornecedor);
  return Results.Ok();
});

app.MapDelete("/fornecedores/{id}", async (int id, FornecedorService fornecedorService) =>
{
  await fornecedorService.DeleteFornecedorAsync(id);
  return Results.Ok();
});

app.Run();
