# iPool Auth API

A layered ASP.NET Core REST API with JWT authentication, PBKDF2 password
hashing and a build-free React frontend served by the API itself.

## Overview

A user-management service built around the parts of an authentication flow
that are easy to get wrong:

1. **Register and sign in** against SQL Server, returning a signed JWT.
2. **Store passwords** as PBKDF2-SHA256 derivations with a per-user random
   salt — never as plaintext or a bare hash.
3. **Protect the user endpoints** so listing and deleting require a valid
   Bearer token.
4. **Document and exercise the API** through Scalar, with the Bearer scheme
   declared in the OpenAPI document so the token can be pasted straight
   into the UI.

The layering is deliberate: controllers hold no business logic, services
never touch the database, and repositories are the only code that talks to
Entity Framework. The React frontend consumes that same API and is served as
static files by the backend itself, so `dotnet run` starts the whole stack —
there is no second process and no Node toolchain.

## Stack

- **.NET 10** — ASP.NET Core Web API, nullable reference types enabled
- **Entity Framework Core 10** — SQL Server provider
- **SQL Server LocalDB** — zero-configuration local database
- **JWT Bearer** (`Microsoft.AspNetCore.Authentication.JwtBearer`) — HMAC-SHA256
- **Scalar** — OpenAPI reference UI at `/scalar/v1`
- **React 18 + Bootstrap 5** — loaded from CDN as UMD bundles with in-browser
  Babel, so the frontend has no build step and no `node_modules`

## Project structure

```
.
├── ipoolBackend/
│   ├── Controllers/          # HTTP surface only: Auth, Users, Health, Info
│   ├── Services/             # business logic: Auth, User, Token, PasswordHasher
│   ├── Repositories/         # data access, the only layer touching EF
│   ├── Data/
│   │   ├── AppDbContext.cs
│   │   ├── DbInitializer.cs
│   │   └── Configurations/   # per-entity EF configuration
│   ├── DTOs/                 # request and response contracts
│   ├── Models/               # domain entities
│   ├── Options/              # Jwt, PasswordHash, AppInfo settings
│   ├── Extensions/           # DI and authentication wiring
│   ├── scripts/              # 01-08 SQL scripts: create, seed, reset, drop
│   └── decisions.md          # technical decision record
└── frontend/
    ├── index.html            # loads React + Babel from CDN
    ├── app.js                # screen router and auth state
    ├── screens/              # Login, Register, ApiOptions
    ├── components/           # Alert, FormInput, FormSelect
    └── hooks/                # useLoginForm, useRegisterForm, useApiOptions
```

## Setup

**Prerequisites:** .NET SDK 10, SQL Server LocalDB, and `sqlcmd`.
SQL Server Management Studio is optional, for inspecting the data.

**1. Confirm the LocalDB instance**

```powershell
& "C:\Program Files\Microsoft SQL Server\160\Tools\Binn\SqlLocalDB.exe" info
```

`MSSQLLocalDB` should appear in the output.

**2. Start the instance**

```powershell
& "C:\Program Files\Microsoft SQL Server\160\Tools\Binn\SqlLocalDB.exe" start MSSQLLocalDB
```

**3. Create the database, tables and indexes**

From the `ipoolBackend` folder:

```powershell
& "C:\Program Files\Sqlcmd\sqlcmd.exe" -S "(localdb)\MSSQLLocalDB" -i "scripts\01_create_database.sql"
```

```powershell
& "C:\Program Files\Sqlcmd\sqlcmd.exe" -S "(localdb)\MSSQLLocalDB" -i "scripts\02_create_tables.sql"
```

```powershell
& "C:\Program Files\Sqlcmd\sqlcmd.exe" -S "(localdb)\MSSQLLocalDB" -i "scripts\03_create_indexes.sql"
```

> If a named-pipe error appears, read the pipe name from
> `SqlLocalDB.exe info MSSQLLocalDB` and pass it as
> `-S "np:\\.\pipe\LOCALDB#...\tsql\query"`.

**4. Set the JWT signing key**

`appsettings.json` ships with the placeholder
`CHANGE_ME_TO_A_LONG_RANDOM_SECRET`. Replace it with a long random string
before running — a short key makes HMAC-SHA256 signatures cheap to forge.

## Running

```bash
dotnet run
```

| Surface | URL |
|---|---|
| React frontend | http://localhost:5000 |
| Scalar API reference | http://localhost:5000/scalar/v1 |

In Scalar, open **Authentication**, choose **Bearer**, and paste the token
returned by `/api/auth/login` to unlock the protected endpoints.

## API reference

| Method | Route | Auth | Description |
|---|---|---|---|
| `POST` | `/api/auth/register` | — | Create a user, returns a token |
| `POST` | `/api/auth/login` | — | Authenticate, returns a token |
| `GET` | `/api/users` | Bearer | List users |
| `DELETE` | `/api/users/{id}` | Bearer | Delete a user by id |
| `GET` | `/api/health` | — | Liveness probe |
| `GET` | `/api/health/db` | — | Database connectivity probe |
| `GET` | `/api/info` | — | Application name and environment |

Example registration body:

```json
{
  "name": "Test User",
  "email": "test@ipool.com",
  "password": "123456"
}
```

## Screenshots

**Backend — register**

<img width="2072" height="1190" alt="Register endpoint in Scalar" src="https://github.com/user-attachments/assets/c7b58c99-c403-40ed-a8eb-f1ac7815ff44" />

**Backend — login**

<img width="2062" height="1200" alt="Login endpoint returning a JWT" src="https://github.com/user-attachments/assets/9982dd5e-4158-4702-8b76-175e7a01b755" />

**Backend — list users** (requires an authenticated request)

<img width="2085" height="1131" alt="Authenticated user listing" src="https://github.com/user-attachments/assets/549fcd64-ecdc-4ea2-8f6e-846b1032dcb9" />

**Backend — delete by id**, leaving a single user behind

<img width="2103" height="1230" alt="Delete user by id" src="https://github.com/user-attachments/assets/1ad9d0e6-bc0d-4006-ae68-b91ac5a680ff" />
<img width="2087" height="902" alt="User list after the deletion" src="https://github.com/user-attachments/assets/146115ab-0ba3-44d4-b3ba-6fc79839c11f" />

**Frontend — registration**

<img width="2275" height="1336" alt="React registration screen" src="https://github.com/user-attachments/assets/340f5b21-f99f-4c16-8696-e319406140fc" />

**Frontend — signed in**

<img width="2232" height="1236" alt="Signed-in user view" src="https://github.com/user-attachments/assets/fe1e30e8-378c-45e7-9424-34f34195c0de" />

**Frontend — deleting a user**

<img width="2329" height="1210" alt="Deleting a user from the frontend" src="https://github.com/user-attachments/assets/a31691c6-b3dd-447d-a4fb-eb1625a37d99" />

## Design notes

**Why PBKDF2 with the iteration count stored in the hash** — `PasswordHasher`
writes `salt.hash.iterations` as a single string. Embedding the work factor
alongside the digest means the cost can be raised later without invalidating
existing passwords: old hashes keep verifying at their original iteration
count. Verification uses `CryptographicOperations.FixedTimeEquals` so that
comparing digests does not leak information through timing.

**Why a unique index on `NormalizedEmail` rather than on `Email`** — email
comparison should be case-insensitive, but storing the address as typed is
better for display and for sending mail. A separate normalized column lets
the database enforce uniqueness while the original value survives intact.

**Why manual SQL scripts instead of EF migrations** — the scripts are
numbered `01`–`08` and cover create, seed, clear, drop and full reset. For a
project whose point is to be run by someone else on a fresh machine, an
explicit script that can be read before it is executed is easier to trust
than a generated migration.

**Why the frontend has no build step** — React and Babel are loaded from CDN
as UMD bundles and JSX is transpiled in the browser. That trades runtime
performance for the ability to clone the repo and run one command, with no
Node toolchain in the prerequisites. It is the right trade for a repository meant to be
cloned and run, and the wrong one for a deployment; see below.

**Why Scalar over Swagger UI** — the OpenAPI document declares the Bearer
scheme through a document transformer, so the token can be pasted once and
reused across every protected endpoint from the reference page.

## Scope and trade-offs

The project is scoped to one thing done properly — issuing and validating
credentials across clean layer boundaries. The choices that follow from that
scope are worth stating explicitly:

- **Configured for local development.** HTTPS redirection is off and the
  frontend loads React's development bundles, so the whole stack runs with
  `dotnet run` and no Node toolchain. A deployment would terminate TLS at the
  edge and compile the frontend ahead of time.
- **Access tokens only, 60-minute lifetime.** A refresh-token flow is session
  lifecycle, which is a separate problem from authentication; leaving it out
  keeps the token path small enough to read end to end.
- **Schema managed by explicit SQL** rather than EF migrations. The numbered
  scripts can be read before they are executed, which matters more here than
  automated schema drift — the trade is that changes mean editing a script.
- **Flat authorization.** Every authenticated user has identical rights.
  Roles and ownership checks are a deliberate non-goal: they would add a
  policy layer without teaching anything more about authentication.
- **Verified through the API surface.** The endpoints are exercised via
  Scalar and the frontend rather than an automated suite — that is what the
  screenshots above document.

## About

A study project on authentication and layered API design in .NET, built
around a single question: what does a login flow look like when the password
storage, the token issuance and the layer boundaries are all done
deliberately rather than by scaffolding?

The technical decision record lives in
[`ipoolBackend/decisions.md`](ipoolBackend/decisions.md).
