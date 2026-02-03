iPool Backend

API REST em .NET com SQL Server LocalDB, JWT e camadas separadas.

Requisitos
Dotnet SDK 10
SQL Server LocalDB
SQL Server Management Studio (SSMS)
Sqlcmd Tools

Passo a passo para o testador

1) Instalar o LocalDB
Abra o instalador do SQL Server Express e marque LocalDB na seleção de recursos.

2) Confirmar a instância LocalDB
Abra o PowerShell e rode:
C:\Program Files\Microsoft SQL Server\160\Tools\Binn\SqlLocalDB.exe info
Deve aparecer MSSQLLocalDB.

3) Iniciar o LocalDB
No PowerShell:
C:\Program Files\Microsoft SQL Server\160\Tools\Binn\SqlLocalDB.exe start MSSQLLocalDB

4) Executar os scripts SQL
No PowerShell, dentro da pasta ipoolBackend:
C:\Program Files\Sqlcmd\sqlcmd.exe -S "(localdb)\MSSQLLocalDB" -i "scripts\01_create_database.sql"
C:\Program Files\Sqlcmd\sqlcmd.exe -S "(localdb)\MSSQLLocalDB" -i "scripts\02_create_tables.sql"
C:\Program Files\Sqlcmd\sqlcmd.exe -S "(localdb)\MSSQLLocalDB" -i "scripts\03_create_indexes.sql"

Se o sqlcmd não existir, instale o Sqlcmd Tools e tente novamente.

Se der erro de pipe, use o nome do pipe retornado por:
C:\Program Files\Microsoft SQL Server\160\Tools\Binn\SqlLocalDB.exe info MSSQLLocalDB
E rode o sqlcmd com -S "np:\\.\pipe\LOCALDB#...\tsql\query".

5) Rodar a API
Entre na pasta ipoolBackend e rode:
dotnet run

Depois disso, o frontend React abre em:
http://localhost:5000

6) Abrir a documentação Scalar
http://localhost:5000/scalar/v1

7) Criar usuário
Endpoint: POST /api/auth/register
Body de exemplo:
name: Usuario Teste
email: teste@ipool.com
password: 123456

8) Login
Endpoint: POST /api/auth/login
Use o token da resposta como Bearer.

No Scalar, clique em Authentication e escolha Bearer, depois cole o token.

9) Testar listagem de usuários
GET /api/users
Header Authorization: Bearer token

10) Excluir usuário
DELETE /api/users/{id}
Header Authorization: Bearer token
Use o id retornado na listagem de usuários.

11) Ver o banco no SSMS
Conectar em: (localdb)\MSSQLLocalDB
Banco: iPoolDb
Tabela: dbo.Users
Clique com o botão direito em dbo.Users e selecione Selecionar 1000 Linhas Superiores.

Endpoints
POST /api/auth/register
POST /api/auth/login
GET /api/users
DELETE /api/users/{id}
GET /api/health
GET /api/health/db
GET /api/info