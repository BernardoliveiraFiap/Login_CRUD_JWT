iPool Backend

API REST em .NET com SQL Server LocalDB, JWT e camadas separadas.

Requisitos
Dotnet SDK 10
SQL Server LocalDB
SQL Server Management Studio (SSMS)

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

Se der erro de pipe, use o nome do pipe retornado por:
C:\Program Files\Microsoft SQL Server\160\Tools\Binn\SqlLocalDB.exe info MSSQLLocalDB
E rode o sqlcmd com -S "np:\\.\pipe\LOCALDB#...\tsql\query".

5) Rodar a API
Entre na pasta ipoolBackend e rode:
dotnet run

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

9) Testar endpoint protegido
GET /api/users/me
Header Authorization: Bearer token

10) Ver o banco no SSMS
Conectar em: (localdb)\MSSQLLocalDB
Banco: iPoolDb
Tabela: dbo.Users
Clique com o botão direito em dbo.Users e selecione Selecionar 1000 Linhas Superiores.

Endpoints
POST /api/auth/register
POST /api/auth/login
GET /api/users/me
GET /api/health
GET /api/health/db
GET /api/info