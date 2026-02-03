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
GET /api/info Backend
#Registro
<img width="2072" height="1190" alt="image" src="https://github.com/user-attachments/assets/c7b58c99-c403-40ed-a8eb-f1ac7815ff44" />

#Login
<img width="2062" height="1200" alt="image" src="https://github.com/user-attachments/assets/9982dd5e-4158-4702-8b76-175e7a01b755" />

#Listar usuários (tem que fazer o login antes)
<img width="2085" height="1131" alt="image" src="https://github.com/user-attachments/assets/549fcd64-ecdc-4ea2-8f6e-846b1032dcb9" />

#deletar com o id do usuário, restou apenas 1 agora.
<img width="2103" height="1230" alt="image" src="https://github.com/user-attachments/assets/1ad9d0e6-bc0d-4006-ae68-b91ac5a680ff" />
<img width="2087" height="902" alt="image" src="https://github.com/user-attachments/assets/146115ab-0ba3-44d4-b3ba-6fc79839c11f" />



#ipoolLogin frontend CADASTRO

<img width="2275" height="1336" alt="image" src="https://github.com/user-attachments/assets/340f5b21-f99f-4c16-8696-e319406140fc" />


#Usuário logado
<img width="2232" height="1236" alt="image" src="https://github.com/user-attachments/assets/fe1e30e8-378c-45e7-9424-34f34195c0de" />

#deletando alguém
<img width="2329" height="1210" alt="image" src="https://github.com/user-attachments/assets/a31691c6-b3dd-447d-a4fb-eb1625a37d99" />




