iPool Backend - Decisões Técnicas

Resumo
Este documento descreve as principais decisões técnicas do backend iPool, a estrutura de diretórios e como os componentes se conectam.

Objetivo do projeto
API REST em .NET com persistência em SQL Server LocalDB, autenticação JWT e separação clara de camadas (Controller, Service, Repository).

Stack escolhida
- .NET 10 (ASP.NET Core Web API)
- SQL Server LocalDB
- JWT para autenticação
- Scalar para documentação OpenAPI

Estrutura de diretórios
- Controllers
  Controladores HTTP. Contêm endpoints e regras mínimas de entrada/saída.
  Exemplos: AuthController, UsersController, HealthController.

- Services
  Lógica de negócio. Não acessa o banco diretamente.
  Exemplos: AuthService, UserService, TokenService, PasswordHasher.

- Repositories
  Acesso a dados. Encapsula consultas no banco.
  Exemplos: UserRepository.

- Data
  AppDbContext e configurações do EF Core. Também contém inicializador do banco.
  Configurações específicas por entidade ficam em Data/Configurations.

- DTOs
  Modelos de entrada e saída da API (requests e responses).

- Models
  Entidades do domínio que refletem as tabelas do banco.

- Options
  Configurações da aplicação (JWT, PasswordHash, AppInfo).

- Extensions
  Extensões de DI para centralizar configurações de serviços e autenticação.

- scripts
  Scripts SQL manuais para criação do banco, tabelas e índices.

Arquitetura e fluxo
1) Controller recebe a requisição e valida o modelo.
2) Controller chama Service.
3) Service usa Repository para buscar ou persistir dados.
4) Repository usa AppDbContext para acessar o banco.

Autenticação
- JWT com claims padrão (sub, email, unique_name).
- Tokens emitidos no login e no cadastro.
- Endpoints protegidos exigem Bearer token.

Banco de dados
- SQL Server LocalDB como padrão.
- Tabela principal: Users.
- Índice único por NormalizedEmail.

Documentação
- Scalar expõe a documentação em /scalar/v1.
- OpenAPI configurado com esquema Bearer.

Pontos de atenção
- JWT Key deve ser longa e segura.
- Em ambiente local, LocalDB deve estar instalado e iniciado.
- Os scripts em scripts/ são úteis para criação manual do banco.

Motivações das escolhas
- LocalDB: simples para testes locais sem configuração de servidor.
- Repository/Service: separa regras de negócio do acesso a dados.
- JWT: padrão simples para autenticação stateless.
- Scalar: interface leve para documentação e testes.
