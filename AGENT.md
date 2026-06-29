# ROH - Reign of Humanae

MMORPG com backend em microsserviços .NET 10 + cliente Unity 6 (HDRP).

## Estrutura do Projeto

```
ROH-ReignOfHumanae/
├── src/
│   ├── Gateway/ROH.Gateway/          # API Gateway (porta 9001 HTTP, 9002 gRPC)
│   ├── Api/                           # Minimal APIs (cada uma roda independe)
│   │   ├── ROH.Api.Account           # porta 9102 / 9202 gRPC
│   │   ├── ROH.Api.Login             # porta 9103 / 9203 gRPC
│   │   ├── ROH.Api.Version           # porta 9101 / 9201 gRPC
│   │   ├── ROH.Api.VersionFiles      # porta 9100 / 9200 gRPC
│   │   ├── ROH.Api.Log               # porta 9104 / 9204 gRPC
│   │   ├── ROH.Api.Player            # porta 9105
│   │   └── ROH.Api.PlayerSync.State  # porta 9210 gRPC
│   ├── Service/                       # Lógica de negócio (gRPC)
│   ├── Database/                      # EF Core DbContexts + Repositories
│   │   ├── ROH.Context.Account       # PostgreSQL
│   │   ├── ROH.Context.Version       # PostgreSQL
│   │   ├── ROH.Context.File          # PostgreSQL
│   │   ├── ROH.Context.Log           # PostgreSQL
│   │   ├── ROH.Context.Player        # PostgreSQL
│   │   ├── ROH.Context.Player.Mongo  # MongoDB (posições, chat)
│   │   └── ROH.Context.Player.Redis  # Redis (cache)
│   ├── Common/                        # Bibliotecas compartilhadas
│   │   ├── ROH.Utils                 # Utilitários (netstandard2.1, usado pelo Unity)
│   │   ├── ROH.StandardModels        # DTOs
│   │   ├── ROH.Mapper                # AutoMapper profiles
│   │   ├── ROH.Contracts             # Contratos WebSocket
│   │   ├── ROH.Protos                # Definições gRPC (.proto)
│   │   └── ROH.Models                # Modelos antigos
│   ├── Site/ROH.Site/                # Blazor Server + WASM (admin, porta 9010)
│   ├── Launcher/ROH.Launcher/        # .NET MAUI (desktop)
│   ├── Worker/                       # Background workers
│   ├── Unity/ReignOfHumanae.Unity/   # Cliente Unity 3D
│   └── Test/ROH.Test/                # Testes unitários (xUnit)
├── docs/                              # Documentação
├── RUNDOCKER.ps1                      # Script Docker (Windows)
├── RUNDOCKER.sh                       # Script Docker (Linux)
└── ReignOfHumanae.sln                 # Solution .NET
```

## Stack

| Camada | Tecnologia |
|--------|-----------|
| Runtime | .NET 10 (ASP.NET Core, Minimal APIs) |
| ORM | Entity Framework Core + Npgsql (PostgreSQL) |
| NoSQL | MongoDB (posições/chat), Redis (cache) |
| Real-time | WebSocket + MessagePack |
| gRPC | gRPC-Web, protobuf |
| Frontend admin | Blazor Server + MudBlazor + CoronaUI |
| Launcher | .NET MAUI |
| Game engine | Unity 6 (HDRP, IL2CPP, .NET Standard 2.1) |
| Container | Docker (sem docker-compose, scripts .ps1/.sh) |
| CI | GitHub Actions (dotnet test) |

## Como Rodar

### Backend completo (Docker)
```powershell
./RUNDOCKER.ps1
```
Sobe: Gateway (9001), Blazor (9010), APIs (Account, Login, Version, VersionFiles, Log) + PostgreSQL.

### Variáveis de ambiente essenciais
Cada API lê connection strings de env vars (ex: `ROH_DATABASE_CONNECTION_STRING_ACCOUNT`, `ROH_DATABASE_CONNECTION_STRING_VERSION`, `ROH_DATABASE_CONNECTION_STRING_FILE`, etc.). A chave JWT vem de `ROH_KEY_TOKEN`.

### Individualmente (dev)
```powershell
$env:ROH_DATABASE_CONNECTION_STRING_ACCOUNT = "Host=localhost;..."; dotnet run --project src/Api/ROH.Api.Account
```

## Arquitetura

```
[Unity/Maui/Browser] → Gateway (9001) → APIs internas (gRPC/HTTP)
                                        → PostgreSQL (contextos separados)
                                        → MongoDB (posições, chat)
                                        → Redis (cache)
```

- Gateway roteia chamadas, faz autenticação JWT e gerencia WebSockets
- Cada API é independente, usa seu próprio DbContext e banco
- Comunicação entre serviços via gRPC
- Versões seguem `Version.Release.Review` (major.minor.patch)

## Banco de Dados

### EF Core Migrations
Todos os contextos têm migrations já geradas. Na inicialização, cada API executa automaticamente:
```csharp
db.Database.Migrate();  // Aplica migrations pendentes (idempotente)
```

### Connection Strings
Por env vars (nunca em appsettings.json):
- `ROH_DATABASE_CONNECTION_STRING_ACCOUNT` — Account, Login APIs
- `ROH_DATABASE_CONNECTION_STRING_VERSION` — Version API
- `ROH_DATABASE_CONNECTION_STRING_FILE` — VersionFiles API
- `ROH_DATABASE_CONNECTION_STRING_LOG` — Log API
- `ROH_DATABASE_CONNECTION_STRING_PLAYER` — Player API, Gateway
- `ROH_MONGO_PLAYER_CONNECTION_STRING` — Gateway, PlayerSync.State
- `ROH_REDIS_PLAYER_CONNECTION_STRING` — PlayerSync.State

### Para gerar nova migration
```powershell
$env:ROH_DATABASE_CONNECTION_STRING_ACCOUNT = "Host=..."; dotnet ef migrations add Nome --project src/Database/ROH.Context.Account
```

## Convenções de Código

- **Sem comentários** no código (salvo copyright header obrigatório)
- `record` para entidades e DTOs
- `record` com posicional (ex: `record GameFile(long Id, Guid Guid, ...)`)
- AutoMapper para mapeamento entidade ↔ DTO (profiles em `ROH.Mapper`)
- FluentValidation para validação de modelos
- Repositories com interface + implementação
- Injeção de dependência via construtor
- Async/await com `ConfigureAwait(false)` nas APIs
- `IServiceScope` para escopo em startup
- Projetos no padrão `ROH.{Camada}.{Dominio}`

## API Gateway

- Porta 9001 (HTTP/1.1 + HTTP/2)
- JWT Bearer (chave de `ROH_KEY_TOKEN`)
- WebSocket em `/ws` com keepalive 15s
- MessagePack para mensagens em tempo real
- Rotas: `/Api/Version/*`, `/api/VersionFile/*`, etc.

## Sistema de Versões (Updater/Launcher)

- Versão semântica: `Version.Release.Review`
- Admin cria versão (rascunho), faz upload de arquivos, depois marca como "Released"
- Arquivos salvos em `ROHUpdateFiles/{version}/` no disco
- Launcher baixa arquivos via `DownloadFileRaw` (com resume + SHA256 checksum)
- Unity UpdateService faz update incremental no `persistentDataPath`
- Versão local salva em `Assets/config` (Unity) ou `launcher-settings.json` (Launcher)

## Regras para o Agente

1. **Leia o contexto** antes de editar qualquer arquivo (entenda imports, estilo, padrões)
2. **Siga as convenções** do projeto (sem comentários, registros, async/await)
3. **Não adicione comentários** ao código a menos que o usuário peça explicitamente
4. **Não crie arquivos novos** a menos que seja absolutamente necessário (prefira editar existentes)
5. **Não adicione emojis** ao código ou documentação
6. **Não faça commit** a menos que o usuário solicite
7. **Verifique o build** após fazer alterações (`dotnet build`)
8. **Não adicione pacotes NuGet** desnecessários — verifique se a dependência já existe como transitiva
9. **Connection strings** nunca em appsettings.json — sempre via environment variables
10. **Prefira usar as ferramentas** Read, Glob, Grep, Edit, Write em vez de comandos bash para manipular arquivos

## Comandos Úteis

```powershell
# Build de um projeto específico
dotnet build src/Api/ROH.Api.Account/ROH.Api.Account.csproj

# Build da solution toda
dotnet build ReignOfHumanae.sln

# Testes
dotnet test src/Test/ROH.Test/ROH.Test.csproj

# Docker (Windows)
./RUNDOCKER.ps1

# Docker (Linux)
./RUNDOCKER.sh
```

## Portas dos Serviços

| Serviço | Porta HTTP | Porta gRPC |
|---------|-----------|------------|
| Gateway | 9001 | 9002 |
| Blazor Site | 9010 | — |
| VersionFiles API | 9100 | 9200 |
| Version API | 9101 | 9201 |
| Account API | 9102 | 9202 |
| Login API | 9103 | 9203 |
| Log API | 9104 | 9204 |
| Player API | 9105 | — |
| PlayerSync.State | — | 9210 |
