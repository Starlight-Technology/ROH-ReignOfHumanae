# ROH - Reign Of Humanae

ROH is a lightweight, modular set of microservices and clients that make up a multiplayer game backend and tools used during development.

This repository contains multiple projects: gateway, APIs, gRPC services, Blazor server, utilities and Unity client integration samples.

## Status

Work in progress. Active development on branch `Feature/GetOtherPlayerPosition`.

## Contents

- `src/Gateway/ROH.Gateway` - API gateway that forwards HTTP and gRPC calls to backend services and exposes gRPC-Web for browser/Unity clients.
- `src/Api/*` - Minimal APIs that implement business logic for account, login, versioning and more.
- `src/Service/*` - Backend services, many exposing gRPC endpoints consumed by the gateway.
- `src/Blazor/ROH.Blazor.Server` - Blazor Server app for admin and dashboards.
- `src/Common/ROH.Utils` - Utility library used by multiple projects.
- `src/Database/*` - Database contexts and repositories.
- `src/unity/ReignOfHumanae.Unity` - Unity game scripts.
- `src/Test/ROH.Test` - Unit tests.

## License

This project is open source and licensed under the MIT License. See `LICENSE` for details.

## Why MIT

Choosing an OSI-approved license like MIT makes the project easier to adopt, permits commercial use, and allows platforms like Tailscale to recognize the project as open source.

## Usage

- Gateway listens on ports 9001 (HTTP/1.1) and 9002 (HTTP/1.1 + HTTP/2) by default.
- gRPC services run on dedicated ports, many configured to use HTTP/2.

### Unity (IL2CPP, .NET Standard 2.1)

Unity builds using IL2CPP and .NET Standard 2.1 must use gRPC-Web compatible handlers. Use `YetAnotherHttpHandler` (Cysharp) in the Unity client. Do not use Microsoft's `GrpcWebHandler` because it depends on .NET APIs not available in Unity.

Example (Unity):

```csharp
using Grpc.Net.Client;
using Cysharp.Net.Http;

var handler = new YetAnotherHttpHandler
{
    Http2Only = false,
    SkipCertificateVerification = true
};
_channel = GrpcChannel.ForAddress(gateway, new GrpcChannelOptions { HttpHandler = handler });
```

## Contributing

Contributions are welcome. Please open issues and pull requests. When contributing, follow the existing code style and include tests where possible.

## Contact

Author: Starlight-Technology (https://github.com/Starlight-Technology/ROH-ReignOfHumanae)
