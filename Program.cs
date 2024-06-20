using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Loja.data;
using Loja.models;
using Loja.Services;
using Loja.data;
using Loja.services;
using Loja.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Configuração do serviço de autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("qwertyuiopasdfghjklzxcvbnmqwerty")),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero // Remover o ClockSkew para que o token expire exatamente na expiração definida
      };
    });

// Configuração do serviço de autorização
builder.Services.AddAuthorization();

// Adição de outros serviços ao contêiner
builder.Services.AddControllers();
builder.Services.AddDbContext<LojaDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 26))));

// Registro de serviços necessários
builder.Services.AddScoped<UsuariosService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<FornecedorService>();
builder.Services.AddScoped<ClientesService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<VendaService>();
builder.Services.AddScoped<DepositoService>();

// Configuração do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware de roteamento e tratamento de erros
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware de autenticação JWT e autorização
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Endpoint para login e geração de token JWT
app.MapPost("/login", async (HttpContext context, UsuariosService usuariosService, TokenService tokenService) =>
{
  using var reader = new StreamReader(context.Request.Body);
  var body = await reader.ReadToEndAsync();
  var json = JsonDocument.Parse(body);
  var email = json.RootElement.GetProperty("email").GetString();
  var senha = json.RootElement.GetProperty("senha").GetString();

  var usuario = await usuariosService.GetUsuarioByEmailAsync(email);
  if (usuario == null || usuario.Senha != senha)
  {
    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    await context.Response.WriteAsync("Credenciais inválidas");
    return;
  }

  var token = tokenService.GenerateToken(email); // Geração do token JWT usando TokenService
  await context.Response.WriteAsync(token);
});

// Endpoints para CRUD de usuários
app.MapPost("/usuarios", async (Usuario usuario, UsuariosService usuariosService) =>
{
  await usuariosService.AddUsuarioAsync(usuario);
  return Results.Created($"/usuarios/{usuario.Id}", usuario);
}).RequireAuthorization();

app.MapGet("/usuarios", async (UsuariosService usuariosService) =>
{
  var usuarios = await usuariosService.GetAllUsuariosAsync();
  return Results.Ok(usuarios);
}).RequireAuthorization();

app.MapGet("/usuarios/{id}", async (int id, UsuariosService usuariosService) =>
{
  var usuario = await usuariosService.GetUsuarioByIdAsync(id);
  return usuario != null ? Results.Ok(usuario) : Results.NotFound($"Usuário com ID {id} não encontrado.");
}).RequireAuthorization();

app.MapPut("/usuarios/{id}", async (int id, Usuario usuario, UsuariosService usuariosService) =>
{
  if (id != usuario.Id)
  {
    return Results.BadRequest("ID do usuário não corresponde.");
  }

  await usuariosService.UpdateUsuarioAsync(usuario);
  return Results.Ok();
}).RequireAuthorization();

app.MapDelete("/usuarios/{id}", async (int id, UsuariosService usuariosService) =>
{
  await usuariosService.DeleteUsuarioAsync(id);
  return Results.Ok();
}).RequireAuthorization();

// Endpoints para CRUD de produtos
app.MapGet("/produtos", async (ProductService productService) =>
{
  var produtos = await productService.GetAllProductsAsync();
  return Results.Ok(produtos);
}).RequireAuthorization();

app.MapGet("/produtos/{id}", async (int id, ProductService productService) =>
{
  var produto = await productService.GetProductByIdAsync(id);
  return produto != null ? Results.Ok(produto) : Results.NotFound($"Produto com ID {id} não encontrado.");
}).RequireAuthorization();

app.MapPost("/produtos", async (Produto produto, ProductService productService) =>
{
  await productService.AddProductAsync(produto);
  return Results.Created($"/produtos/{produto.Id}", produto);
}).RequireAuthorization();

app.MapPut("/produtos/{id}", async (int id, Produto produto, ProductService productService) =>
{
  if (id != produto.Id)
  {
    return Results.BadRequest("ID do produto não corresponde.");
  }

  await productService.UpdateProductAsync(produto);
  return Results.Ok();
}).RequireAuthorization();

app.MapDelete("/produtos/{id}", async (int id, ProductService productService) =>
{
  await productService.DeleteProductAsync(id);
  return Results.Ok();
}).RequireAuthorization();

// Endpoints para CRUD de clientes
app.MapGet("/clientes", async (ClientesService clientesService) =>
{
  var clientes = await clientesService.GetAllClientesAsync();
  return Results.Ok(clientes);
}).RequireAuthorization();

app.MapGet("/clientes/{id}", async (int id, ClientesService clientesService) =>
{
  var cliente = await clientesService.GetClientesByIdAsync(id);
  return cliente != null ? Results.Ok(cliente) : Results.NotFound($"Cliente com ID {id} não encontrado.");
}).RequireAuthorization();

app.MapPost("/clientes", async (Cliente cliente, ClientesService clientesService) =>
{
  await clientesService.AddClientesAsync(cliente);
  return Results.Created($"/clientes/{cliente.Id}", cliente);
}).RequireAuthorization();

app.MapPut("/clientes/{id}", async (int id, Cliente cliente, ClientesService clientesService) =>
{
  if (id != cliente.Id)
  {
    return Results.BadRequest("ID do cliente não corresponde.");
  }

  await clientesService.UpdateClientesAsync(cliente);
  return Results.Ok();
}).RequireAuthorization();

app.MapDelete("/clientes/{id}", async (int id, ClientesService clientesService) =>
{
  await clientesService.DeleteClientesAsync(id);
  return Results.Ok();
}).RequireAuthorization();

// Endpoints para CRUD de fornecedores
app.MapGet("/fornecedores", async (FornecedorService fornecedorService) =>
{
  var fornecedores = await fornecedorService.GetAllFornecedoresAsync();
  return Results.Ok(fornecedores);
}).RequireAuthorization();

app.MapGet("/fornecedores/{id}", async (int id, FornecedorService fornecedorService) =>
{
  var fornecedor = await fornecedorService.GetFornecedorByIdAsync(id);
  return fornecedor != null ? Results.Ok(fornecedor) : Results.NotFound($"Fornecedor com ID {id} não encontrado.");
}).RequireAuthorization();

app.MapPost("/fornecedores", async (Fornecedor fornecedor, FornecedorService fornecedorService) =>
{
  await fornecedorService.AddFornecedorAsync(fornecedor);
  return Results.Created($"/fornecedores/{fornecedor.Id}", fornecedor);
}).RequireAuthorization();

app.MapPut("/fornecedores/{id}", async (int id, Fornecedor fornecedor, FornecedorService fornecedorService) =>
{
  if (id != fornecedor.Id)
  {
    return Results.BadRequest("ID do fornecedor não corresponde.");
  }

  await fornecedorService.UpdateFornecedorAsync(fornecedor);
  return Results.Ok();
}).RequireAuthorization();

app.MapDelete("/fornecedores/{id}", async (int id, FornecedorService fornecedorService) =>
{
  await fornecedorService.DeleteFornecedorAsync(id);
  return Results.Ok();
}).RequireAuthorization();

// Endpoints relacionados às vendas e estoque
app.MapPost("/vendas", async (Venda venda, VendaService vendaService) =>
{
  var clienteExistente = await vendaService.ClienteExistsAsync(venda.ClienteId);
  if (!clienteExistente)
  {
    return Results.BadRequest("Cliente não encontrado.");
  }

  var produtoExistente = await vendaService.ProdutoExistsAsync(venda.ProdutoId);
  if (!produtoExistente)
  {
    return Results.BadRequest("Produto não encontrado.");
  }

  await vendaService.CriarVendaAsync(venda);
  return Results.Created($"/vendas/{venda.Id}", venda);
}).RequireAuthorization();

app.MapGet("/vendas/produto/{id}/detalhada", async (int id, VendaService vendaService) =>
{
  var vendasDetalhadas = await vendaService.GetVendasDetalhadasPorProdutoAsync(id);
  return vendasDetalhadas != null && vendasDetalhadas.Count > 0 ? Results.Ok(vendasDetalhadas) : Results.NotFound($"Não há vendas registradas para o produto com ID {id}.");
}).RequireAuthorization();

app.MapGet("/vendas/produto/{id}/sumarizada", async (int id, VendaService vendaService) =>
{
  var vendasSumarizadas = await vendaService.GetVendasSumarizadasPorProdutoAsync(id);
  return vendasSumarizadas != null ? Results.Ok(vendasSumarizadas) : Results.NotFound($"Não há vendas registradas para o produto com ID {id}.");
}).RequireAuthorization();

app.MapGet("/vendas/cliente/{id}/detalhada", async (int id, VendaService vendaService) =>
{
  var vendasDetalhadas = await vendaService.GetVendasDetalhadasPorClienteAsync(id);
  return vendasDetalhadas != null && vendasDetalhadas.Count > 0 ? Results.Ok(vendasDetalhadas) : Results.NotFound($"Não há vendas registradas para o cliente com ID {id}.");
}).RequireAuthorization();

app.MapGet("/vendas/cliente/{id}/sumarizada", async (int id, VendaService vendaService) =>
{
  var vendasSumarizadas = await vendaService.GetVendasSumarizadasPorClienteAsync(id);
  return vendasSumarizadas != null ? Results.Ok(vendasSumarizadas) : Results.NotFound($"Não há vendas registradas para o cliente com ID {id}.");
}).RequireAuthorization();

// Endpoints para o serviço de depósito
app.MapGet("/estoque/depósito/{id}/sumarizada", async (int id, DepositoService depositoService) =>
{
  var produtosNoDeposito = await depositoService.GetProdutosNoDepositoAsync(id);
  return produtosNoDeposito != null ? Results.Ok(produtosNoDeposito) : Results.NotFound($"Não há produtos registrados no depósito com ID {id}.");
}).RequireAuthorization();

app.MapGet("/estoque/produto/{id}/quantidade", async (int id, DepositoService depositoService) =>
{
  var quantidadeProduto = await depositoService.GetQuantidadeProdutoAsync(id);
  return quantidadeProduto != null ? Results.Ok(quantidadeProduto) : Results.NotFound($"Produto com ID {id} não encontrado no depósito.");
}).RequireAuthorization();

// Middleware de configuração de endpoints
app.UseEndpoints(endpoints =>
{
  endpoints.MapControllers();
});

app.Run();
