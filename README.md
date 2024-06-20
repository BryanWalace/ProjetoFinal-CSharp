# Sistema de Gestão de Loja

Este projeto é um sistema de gestão de uma loja fictícia, desenvolvido em C# com ASP.NET Core, utilizando JWT para autenticação e MySQL como banco de dados. Ele oferece endpoints para CRUD de usuários, produtos, clientes, fornecedores, vendas e gerenciamento de estoque.

## Funcionalidades

- Autenticação JWT para login e geração de tokens.
- CRUD completo para usuários, produtos, clientes, fornecedores e vendas.
- Consultas detalhadas e sumarizadas de vendas por produto e cliente.
- Gerenciamento de estoque com consultas de produtos por depósito e quantidade.

## Tecnologias Utilizadas

- ASP.NET Core
- Entity Framework Core com MySQL
- JWT (JSON Web Tokens) para autenticação
- Swagger/OpenAPI para documentação de API
- MySQL como banco de dados

## Pré-requisitos

- .NET 6 SDK
- MySQL Server
- Postman ou ferramenta similar para testar os endpoints

## Instalação e Uso

### Configuração do Banco de Dados

1. **Configuração da Conexão:**

   - Configure a conexão com o banco de dados MySQL no arquivo `appsettings.json`:
     ```json
     {
       "ConnectionStrings": {
         "DefaultConnection": "server=localhost;port=3306;database=LojaDb;user=root;password=senha;"
       }
     }
     ```
     Substitua `senha` pela senha do seu banco de dados MySQL.

2. **Aplicação das Migrações:**
   - Execute as migrações para criar a estrutura do banco de dados:
     ```bash
     dotnet ef database update
     ```

### Execução da Aplicação

1. **Executar o Projeto:**

   - No diretório raiz do projeto, execute:
     ```bash
     dotnet run
     ```

2. **Acesso à Documentação da API:**

   - Acesse a documentação da API Swagger em `https://localhost:<porta>/swagger/index.html`.

3. **Testar os Endpoints:**
   - Utilize o Postman ou ferramenta similar para testar os endpoints da API. Exemplos de endpoints disponíveis:
     - Login: `POST /login`
     - CRUD Usuários: `POST /usuarios`, `GET /usuarios`, `PUT /usuarios/{id}`, `DELETE /usuarios/{id}`
     - CRUD Produtos, Clientes, Fornecedores, Vendas, etc.

## Contribuição

Contribuições são bem-vindas! Sinta-se à vontade para abrir um Pull Request com melhorias, correções de bugs ou novas funcionalidades.

## Licença

Este projeto está licenciado sob a Licença MIT - veja o arquivo [LICENSE](LICENSE) para mais detalhes.
