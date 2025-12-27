# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["src/Worker/ROH.Worker.GetNearbyPlayer/ROH.Worker.GetNearbyPlayer.csproj", "src/Worker/ROH.Worker.GetNearbyPlayer/"]
COPY ["src/Common/ROH.Contracts/ROH.Contracts.csproj", "src/Common/ROH.Contracts/"]
COPY ["src/Common/ROH.StandardModels/ROH.StandardModels.csproj", "src/Common/ROH.StandardModels/"]
COPY ["src/Common/ROH.Utils/ROH.Utils.csproj", "src/Common/ROH.Utils/"]
RUN dotnet restore "./src/Worker/ROH.Worker.GetNearbyPlayer/ROH.Worker.GetNearbyPlayer.csproj"
COPY . .
WORKDIR "/src/src/Worker/ROH.Worker.GetNearbyPlayer"
RUN dotnet build "./ROH.Worker.GetNearbyPlayer.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ROH.Worker.GetNearbyPlayer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ROH.Worker.GetNearbyPlayer.dll"]