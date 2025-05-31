# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Api/ROH.Api.PlayerSync.SavePosition/ROH.Api.PlayerSync.SavePosition.csproj", "src/Api/ROH.Api.PlayerSync.SavePosition/"]
COPY ["src/Common/ROH.Protos/ROH.Protos.csproj", "src/Common/ROH.Protos/"]
COPY ["src/Service/ROH.Service.Player.Grpc/ROH.Service.Player.Grpc.csproj", "src/Service/ROH.Service.Player.Grpc/"]
COPY ["src/Database/ROH.Context.Player.Mongo/ROH.Context.Player.Mongo.csproj", "src/Database/ROH.Context.Player.Mongo/"]
COPY ["src/Service/ROH.Service.Exception/ROH.Service.Exception.csproj", "src/Service/ROH.Service.Exception/"]
COPY ["src/Common/ROH.StandardModels/ROH.StandardModels.csproj", "src/Common/ROH.StandardModels/"]
COPY ["src/Common/ROH.Utils/ROH.Utils.csproj", "src/Common/ROH.Utils/"]
COPY ["src/Database/ROH.Context.Log/ROH.Context.Log.csproj", "src/Database/ROH.Context.Log/"]
RUN dotnet restore "./src/Api/ROH.Api.PlayerSync.SavePosition/ROH.Api.PlayerSync.SavePosition.csproj"
COPY . .
WORKDIR "/src/src/Api/ROH.Api.PlayerSync.SavePosition"
RUN dotnet build "./ROH.Api.PlayerSync.SavePosition.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ROH.Api.PlayerSync.SavePosition.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ROH.Api.PlayerSync.SavePosition.dll"]