# FIAP.CloudGame.Users ✅

## Sobre

`FIAP.CloudGame.Users` é um microserviço ASP.NET Core responsável pelo gerenciamento de usuários e suas bibliotecas de jogos (owned games). Ele provê endpoints para registro, autenticação via JWT, operações administrativas (criação e listagem de usuários) e manipulação de jogos pertencentes a usuários.

---

## Arquitetura 🔧

O projeto é organizado em camadas com responsabilidades bem definidas:

- **FIAP.CloudGames.Usuarios.Api** — API HTTP (controllers, configuração do app, Swagger, autenticação).
- **FIAP.CloudGames.Usuarios.Service** — Regras de negócio e validações (serviços).
- **FIAP.CloudGames.Usuarios.Infrastructure** — Persistência (Entity Framework, migrations, repositórios) e integrações (MongoDB para logs).
- **FIAP.CloudGames.Usuarios.Domain** — Entidades, DTOs (Requests/Responses), enums e exceções.

Arquitetura do fluxo principal:

1. Usuário se registra ou é criado por um Admin.
2. Usuário faz login para obter um token JWT.
3. Token é usado em chamadas autenticadas para gerenciar a biblioteca de jogos.

---

## Tecnologias 🧰

- .NET 8 (ASP.NET Core)
- Entity Framework Core (SQL Server)
- JWT (Microsoft.AspNetCore.Authentication.JwtBearer)
- Serilog (logs + sink para MongoDB)
- Swagger (Swashbuckle)
- Docker (Dockerfile já presente na pasta da API)

---

## Configuração 🔧

Principais configurações em `FIAP.CloudGames.Usuarios.Api/appsettings.json` (ou `appsettings.Development.json` / `appsettings.Secrets.json`):

- **ConnectionStrings:DefaultConnection** — string de conexão com SQL Server.
- **Jwt** — configurações para emissão/validação de tokens (Issuer, Audience, Key, ExpireIn, etc.).
- **MongoDB** — connection string usada pelo Serilog para armazenar logs.

Exemplo mínimo (ex: `appsettings.Secrets.json` — NÃO comitar em repositório):

```json
{
	"ConnectionStrings": { "DefaultConnection": "Server=localhost;Database=UsuariosDb;User Id=sa;Password=Your_password123;" },
	"Jwt": { "Issuer": "fiap", "Audience": "fiap", "Key": "uma-chave-secreta-muito-long", "ExpireIn": "01:00:00" },
	"MongoDB": "mongodb://localhost:27017/logs-db"
}
```

---

## Executando 🏃

Pré-requisitos:

- .NET SDK 8.0
- SQL Server (ou uma string de conexão compatível)
- (Opcional) MongoDB para logs

Comandos básicos (na raiz da solução):

```bash
dotnet restore
dotnet build
dotnet run --project FIAP.CloudGames.Usuarios.Api
```

Para aplicar migrations (gerar banco / scripts):

```bash
dotnet ef database update --context DataContext --project FIAP.CloudGames.Usuarios.Infrastructure --startup-project FIAP.CloudGames.Usuarios.Api
```

Executando em Docker (na pasta raiz da solution):

```bash
docker build -t fiap-cloudgames-usuarios -f FIAP.CloudGames.Usuarios.Api/Dockerfile .
docker run -e ASPNETCORE_URLS="http://+:8080" -e ConnectionStrings__DefaultConnection="<sua-connection-string>" -p 8080:8080 fiap-cloudgames-usuarios
```

---

## Endpoints Principais 📡

Base URL: `http://{host}:{port}/api`

- **POST /api/auth/login** — Autentica usuário e retorna token JWT.
	- Request (JSON): `{ "email": "usuario@exemplo.com", "password": "senha" }`
	- Response (200): `{ "data": { "token": "<jwt>", "expireIn": "2025-..." }, ... }`

- **POST /api/user/register** — Registra novo usuário (público).
	- Request: `{ "name": "Nome", "email": "a@b.com", "password": "senha" }`

- **GET /api/user/users** — Lista todos os usuários (somente Admin).

- **POST /api/user/create-user-admin** — Cria usuário com role (somente Admin).

- **PUT /api/user/{id}/role?role=Admin** — Atualiza role de um usuário (somente Admin).

- **GET /api/user/me** — Retorna perfil do usuário autenticado (Bearer Token requerido).

- **POST /api/ownedgame** — Adiciona um jogo à biblioteca do usuário (Bearer Token).
	- Request: `{ "userId": 1, "gameId": 123 }`

- **GET /api/ownedgame/user/{userId}** — Lista jogos de um usuário (Bearer Token e validação de acesso via filtro).

Exemplo de login via curl:

```bash
curl -X POST "http://localhost:5000/api/auth/login" -H "Content-Type: application/json" -d '{"email":"admin@exemplo.com","password":"senha"}'
```

Exemplo de uso do token (substitua <TOKEN>):

```bash
curl -H "Authorization: Bearer <TOKEN>" http://localhost:5000/api/user/me
```

---

## Autenticação 🔐

Autenticação baseada em JWT:

- Obtenha o token via `POST /api/auth/login`.
- Envie o token no header `Authorization: Bearer <token>` para endpoints protegidos.
- Algumas rotas exigem role `Admin` (ex.: listagem e criação de usuários via admin).

---

## Fluxo Principal 🔁

1. Usuário faz `POST /api/user/register` (ou Admin cria via `create-user-admin`).
2. Usuário faz `POST /api/auth/login` para obter JWT.
3. Com o token, o usuário acessa `GET /api/user/me` e adiciona jogos com `POST /api/ownedgame`.
4. Admin pode listar todos os usuários e alterar roles.

---

## Swagger 🧾

Swagger já está configurado (Swashbuckle). Quando a aplicação estiver rodando, acesse:

`http://{host}:{port}/swagger` — Interface interativa para testar endpoints e ver schemas.

---

## Docker 🐳

Existe um `Dockerfile` em `FIAP.CloudGames.Usuarios.Api`. Exemplo de build e run:

```bash
docker build -t fiap-cloudgames-usuarios -f FIAP.CloudGames.Usuarios.Api/Dockerfile .
docker run -p 8080:8080 -e ConnectionStrings__DefaultConnection="<conn>" fiap-cloudgames-usuarios
```

Configure variáveis de ambiente (ex.: `ConnectionStrings__DefaultConnection`, `Jwt__Key`, `MongoDB`) ao executar o container.

---

## Licença 📄

Projeto desenvolvido para FIAP.

---