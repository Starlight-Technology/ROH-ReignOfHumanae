# Relatorio tecnico: persistencia realtime, sessao unica e chat global

## Resumo

O Gateway WebSocket agora autentica conta e personagem, valida a posse do personagem, mantem uma unica sessao ativa por `CharacterId`, restaura a ultima posicao persistida e oferece chat global persistido em MongoDB.

A movimentacao continua autoritativa no servico gRPC existente. O Gateway nao aceita o `PlayerId` enviado pelo cliente como identidade e nao criou uma regra paralela de movimento. Coordenadas e rotacoes invalidas foram incorporadas ao `PlayerValidPositionService`, junto das validacoes existentes de timestamp, velocidade e teleporte.

## Arquivos criados

Contratos compartilhados:

- `src/Common/ROH.Contracts/WebSocket/Chat/ChatChannel.cs`
- `src/Common/ROH.Contracts/WebSocket/Chat/ChatSendMessage.cs`
- `src/Common/ROH.Contracts/WebSocket/Chat/ChatMessage.cs`
- `src/Common/ROH.Contracts/WebSocket/Chat/ChatHistoryRequest.cs`
- `src/Common/ROH.Contracts/WebSocket/Chat/ChatHistoryResponse.cs`
- `src/Common/ROH.Contracts/WebSocket/Chat/ChatErrorMessage.cs`
- `src/Common/ROH.Contracts/WebSocket/Player/InitialPlayerStateMessage.cs`
- `src/Common/ROH.Contracts/WebSocket/Session/DisconnectNoticeMessage.cs`

MongoDB:

- `src/Database/ROH.Context.Player.Mongo/Entities/ChatMessageEntity.cs`
- `src/Database/ROH.Context.Player.Mongo/Interface/IChatRepository.cs`
- `src/Database/ROH.Context.Player.Mongo/Interface/IMongoIndexInitializer.cs`
- `src/Database/ROH.Context.Player.Mongo/Repository/ChatRepository.cs`
- `src/Database/ROH.Context.Player.Mongo/MongoIndexInitializer.cs`

Gateway realtime:

- `src/Gateway/ROH.Gateway/Realtime/AuthenticatedRealtimeIdentity.cs`
- `src/Gateway/ROH.Gateway/Realtime/RealtimeOptions.cs`
- `src/Gateway/ROH.Gateway/Realtime/IRealtimeIdentityService.cs`
- `src/Gateway/ROH.Gateway/Realtime/RealtimeIdentityService.cs`
- `src/Gateway/ROH.Gateway/Realtime/RealtimeClientSession.cs`
- `src/Gateway/ROH.Gateway/Realtime/IRealtimeSessionRegistry.cs`
- `src/Gateway/ROH.Gateway/Realtime/RealtimeSessionRegistry.cs`
- `src/Gateway/ROH.Gateway/Realtime/ChatRateLimiter.cs`
- `src/Gateway/ROH.Gateway/Realtime/ChatValidationResult.cs`
- `src/Gateway/ROH.Gateway/Realtime/ChatMessageValidator.cs`
- `src/Gateway/ROH.Gateway/Realtime/IGlobalChatService.cs`
- `src/Gateway/ROH.Gateway/Realtime/GlobalChatService.cs`
- `src/Gateway/ROH.Gateway/Realtime/ChatCleanupService.cs`
- `src/Gateway/ROH.Gateway/Realtime/MongoIndexHostedService.cs`

Unity:

- `src/Unity/ReignOfHumanae.Unity/Assets/_ROH/Core/Models/Websocket/ChatChannel.cs`
- `src/Unity/ReignOfHumanae.Unity/Assets/_ROH/Core/Models/Websocket/ChatSendMessage.cs`
- `src/Unity/ReignOfHumanae.Unity/Assets/_ROH/Core/Models/Websocket/ChatMessageModel.cs`
- `src/Unity/ReignOfHumanae.Unity/Assets/_ROH/Core/Models/Websocket/ChatHistoryRequest.cs`
- `src/Unity/ReignOfHumanae.Unity/Assets/_ROH/Core/Models/Websocket/ChatHistoryResponse.cs`
- `src/Unity/ReignOfHumanae.Unity/Assets/_ROH/Core/Models/Websocket/ChatErrorMessage.cs`
- `src/Unity/ReignOfHumanae.Unity/Assets/_ROH/Core/Models/Websocket/DisconnectNoticeMessage.cs`
- `src/Unity/ReignOfHumanae.Unity/Assets/_ROH/Core/Models/Websocket/InitialPlayerStateMessage.cs`
- `src/Unity/ReignOfHumanae.Unity/Assets/_ROH/UI/Chat/GlobalChatController.cs`

Testes:

- `src/Test/ROH.Test/Realtime/ChatMessageValidatorTests.cs`
- `src/Test/ROH.Test/Realtime/ChatCleanupServiceTests.cs`
- `src/Test/ROH.Test/Realtime/ChatRateLimiterTests.cs`
- `src/Test/ROH.Test/Realtime/RealtimeSessionRegistryTests.cs`
- `src/Test/ROH.Test/Realtime/RealtimeOptionsTests.cs`

## Arquivos alterados

- `.gitignore`, para permitir somente os novos fontes Unity dentro do diretorio cujo nome termina em `.unity`.
- Contrato gRPC `PlayerPosition.proto` e tipos realtime backend/Unity.
- Modelos da validacao autoritativa em `ROH.StandardModels`.
- Entidades, mapper, contexto, interfaces e repositorio Mongo de posicao.
- `RealtimeConnectionManager`, `Program.cs`, projeto e appsettings do Gateway.
- Servicos gRPC/WebSocket de posicao e cache de jogadores conectados.
- `ROH.Service.WebSocket/WebSocketService.cs`, centralizando o lock de envio por socket.
- Teste existente do `PlayerValidPositionService` e projeto de testes.
- `PlayerService.cs`, `WebSocketService.cs`, `RealtimeEventTypes.cs` e `Assembly-CSharp.csproj` do Unity.

O worktree ja continha uma reorganizacao ampla de pastas Unity e exclusoes de cenas antes desta revisao. Essas mudancas nao foram revertidas.

## Protocolo realtime

Novos tipos:

- `InitialPlayerState`
- `ChatSend`
- `ChatMessage`
- `ChatHistoryRequest`
- `ChatHistoryResponse`
- `ChatError`
- `DisconnectNotice`
- `SystemMessage`, reservado

Os nomes e chaves MessagePack de `SavePlayerPosition`, `SavePlayerPositionResponse` e `GetNearbyPlayers` foram preservados.

A URL WebSocket usa `access_token`, `character_id` e `world_id`. O Gateway valida se o personagem pertence ao GUID da conta presente no JWT.

## Posicao

- O estado realtime continua sendo aprovado pelo `PlayerValidPositionService`.
- O primeiro movimento tambem passa pelo mesmo pipeline autoritativo.
- NaN, Infinity, rotacao invalida e coordenadas fora do limite sao resultados `InvalidCoordinates` desse pipeline.
- Somente respostas aprovadas atualizam a ultima posicao em memoria.
- `persistPosition` e ativado no maximo uma vez por intervalo configurado.
- No disconnect da sessao ativa, o Gateway tenta persistir imediatamente a ultima posicao aprovada.
- O Mongo usa upsert por `CharacterId`; nao cria historico infinito.
- Ao entrar, o Gateway prefere a posicao Mongo. Sem documento Mongo, usa o spawn relacional atual do personagem.
- `InitialPlayerState` sempre envia o transform autoritativo. O Unity aplica esse transform antes de iniciar os envios de movimento.

Campos persistidos: `CharacterId`, `AccountId`, `WorldId`, posicao XYZ, rotacao XYZW e `UpdatedAtUtc`. `PlayerId` e `Timestamp` foram mantidos para compatibilidade.

## Sessao unica

O registro ativo usa `CharacterId` e cada conexao recebe um `ConnectionId`.

Ao substituir:

- a nova sessao entra no registro;
- o socket ativo do personagem passa a ser o novo;
- a ultima posicao aprovada e descarregada;
- a antiga recebe `DisconnectNotice` com `DuplicateLogin`;
- o socket antigo e fechado;
- o `finally` antigo nao remove a nova sessao, pois a remocao compara `ConnectionId`.

O cache de posicao do personagem e preservado durante a troca para manter continuidade da validacao. Ele e removido quando a sessao atualmente ativa encerra.

## Chat e seguranca

Somente `Global` esta implementado. `Private`, `Party`, `Guild` e `System` existem apenas como valores reservados.

O servidor define `SenderCharacterId`, `SenderDisplayName`, `CreatedAtUtc`, tags e alvo. O payload recebido nunca e gravado diretamente.

Validacoes:

- mensagem vazia rejeitada;
- limite de caracteres configuravel;
- quebras de linha, tabs e espacos excessivos normalizados;
- caracteres de controle e formato removidos;
- canal nao implementado rejeitado;
- token bucket por sessao/personagem;
- erro devolvido como `ChatError`, sem derrubar a conexao.

O Unity desativa rich text, limita a lista a 200 linhas e junta historico e mensagens novas no mesmo painel.

## MongoDB e indices

Colecao de posicao: `player_positions`.

- unico esparso por `CharacterId`: `ux_character_id`
- 2dsphere por `Position`: `ix_position_2dsphere`

Colecao de chat: `chat_messages`.

- `Channel + CreatedAtUtc`: `ix_channel_created_at`
- `CreatedAtUtc`: `ix_created_at`
- `SenderCharacterId + CreatedAtUtc`: `ix_sender_created_at`

O worker remove somente documentos com `CreatedAtUtc < UtcNow - retention`. A operacao e idempotente; multiplas instancias podem executar o mesmo delete, embora nao exista eleicao distribuida nesta etapa.

## Configuracao

Arquivos:

- `src/Gateway/ROH.Gateway/appsettings.json`
- `src/Gateway/ROH.Gateway/appsettings.Development.json`

Defaults:

```json
{
  "PlayerPositionSaveIntervalSeconds": 5,
  "ChatMaxMessageLength": 300,
  "ChatHistoryLimit": 50,
  "ChatRateLimitMessagesPerSecond": 1,
  "ChatRateLimitBurst": 3,
  "ChatCleanupIntervalHours": 6,
  "ChatRetentionHours": 72
}
```

Valores invalidos sao normalizados por `RealtimeOptions.ApplySafeDefaults`. Retention nunca fica abaixo de uma hora.

Variaveis existentes: `ROH_KEY_TOKEN`, `ROH_MONGO_PLAYER_CONNECTION_STRING` e `ROH_DATABASE_CONNECTION_STRING_PLAYER`.

## Teste manual no Unity

1. Iniciar PostgreSQL, MongoDB, host `ROH.Api.PlayerSync.State` e Gateway.
2. Abrir o projeto no Unity Editor para regenerar os `.csproj`.
3. Entrar com um personagem e confirmar que ele e reposicionado pelo `InitialPlayerState`.
4. Mover por mais de cinco segundos, sair e entrar novamente; confirmar restauracao da ultima posicao.
5. Abrir dois clientes com o mesmo personagem; confirmar aviso `DuplicateLogin` no primeiro e permanencia do segundo.
6. Abrir dois personagens diferentes; enviar mensagens pelo painel no canto inferior esquerdo e confirmar broadcast.
7. Reconectar e confirmar historico em ordem crescente.
8. Enviar varias mensagens rapidamente e confirmar `ChatError` sem desconexao.
9. Para limpeza, usar retention de uma hora, inserir uma mensagem antiga e aguardar/reiniciar o Gateway; somente a antiga deve ser removida.

## Verificacao automatizada

- `dotnet build src/Gateway/ROH.Gateway/ROH.Gateway.csproj --no-restore`: passou.
- `dotnet build src/Api/ROH.Api.PlayerSync.State/ROH.Api.PlayerSync.State.csproj --no-restore`: passou.
- `dotnet test src/Test/ROH.Test/ROH.Test.csproj --no-restore`: 99 testes passaram.
- `git diff --check`: passou.
- Build CLI do Unity ficou bloqueado por referencias geradas para Unity `6000.3.1f1` e `Microsoft.Unity.Analyzers.dll` ausentes nesta maquina.

## Riscos, pendencias e suposicoes

- Sessao unica e registro de sockets sao em memoria e valem por instancia do Gateway. Escala horizontal exigira coordenacao distribuida.
- O Gateway agora consulta PostgreSQL para validar posse e obter o spawn relacional.
- Foi assumido que o claim JWT `jti` continua contendo o GUID da conta/usuario usado por `Character.GuidAccount`.
- Foi assumido que a posicao relacional do personagem e o spawn/default autoritativo quando ainda nao existe documento Mongo.
- A inicializacao de indices registra erro e permite o Gateway iniciar se o Mongo estiver temporariamente indisponivel.
- O repositorio continua emitindo o aviso conhecido `NU1902` para `SharpCompress 0.30.1`.
- As cenas `Login.unity` e `Login_test.unity`, alem da reorganizacao ampla de assets Unity, ja estavam alteradas no worktree e nao foram restauradas.
