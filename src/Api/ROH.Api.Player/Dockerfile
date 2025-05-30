# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Api/ROH.Api.Player/ROH.Api.Player.csproj", "src/Api/ROH.Api.Player/"]
COPY ["src/Database/ROH.Context.Player/ROH.Context.Player.csproj", "src/Database/ROH.Context.Player/"]
COPY ["src/Service/ROH.Service.Exception/ROH.Service.Exception.csproj", "src/Service/ROH.Service.Exception/"]
COPY ["src/Common/ROH.StandardModels/ROH.StandardModels.csproj", "src/Common/ROH.StandardModels/"]
COPY ["src/Common/ROH.Utils/ROH.Utils.csproj", "src/Common/ROH.Utils/"]
COPY ["src/Database/ROH.Context.Log/ROH.Context.Log.csproj", "src/Database/ROH.Context.Log/"]
COPY ["src/Service/ROH.Service.Player/ROH.Service.Player.csproj", "src/Service/ROH.Service.Player/"]
RUN dotnet restore "./src/Api/ROH.Api.Player/ROH.Api.Player.csproj"
COPY . .
WORKDIR "/src/src/Api/ROH.Api.Player"
RUN dotnet build "./ROH.Api.Player.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ROH.Api.Player.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ROH.Api.Player.dll"]