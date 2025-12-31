# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 9210:9210


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["src/Api/ROH.Api.PlayerSync.State/ROH.Api.PlayerSync.State.csproj", "src/Api/ROH.Api.PlayerSync.State/"]
COPY ["src/Database/ROH.Context.Player.Mongo/ROH.Context.Player.Mongo.csproj", "src/Database/ROH.Context.Player.Mongo/"]
COPY ["src/Database/ROH.Context.Player.Redis/ROH.Context.Player.Redis.csproj", "src/Database/ROH.Context.Player.Redis/"]
COPY ["src/Common/ROH.StandardModels/ROH.StandardModels.csproj", "src/Common/ROH.StandardModels/"]
COPY ["src/Service/ROH.Service.Exception/ROH.Service.Exception.csproj", "src/Service/ROH.Service.Exception/"]
COPY ["src/Common/ROH.Utils/ROH.Utils.csproj", "src/Common/ROH.Utils/"]
COPY ["src/Database/ROH.Context.Log/ROH.Context.Log.csproj", "src/Database/ROH.Context.Log/"]
COPY ["src/Service/ROH.Service.Player.Grpc/ROH.Service.Player.Grpc.csproj", "src/Service/ROH.Service.Player.Grpc/"]
COPY ["src/Common/ROH.Contracts/ROH.Contracts.csproj", "src/Common/ROH.Contracts/"]
RUN dotnet restore "./src/Api/ROH.Api.PlayerSync.State/ROH.Api.PlayerSync.State.csproj"
COPY . .
WORKDIR "/src/src/Api/ROH.Api.PlayerSync.State"
RUN dotnet build "./ROH.Api.PlayerSync.State.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ROH.Api.PlayerSync.State.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ROH.Api.PlayerSync.State.dll"]