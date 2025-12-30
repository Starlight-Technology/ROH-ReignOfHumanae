# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
RUN useradd -u 1010 roh
USER roh 
WORKDIR /app
EXPOSE 9100:9100


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Api/ROH.Api.VersionFiles/ROH.Api.VersionFiles.csproj", "src/Api/ROH.Api.VersionFiles/"]
COPY ["src/Common/ROH.Mapper/ROH.Mapping.csproj", "src/Common/ROH.Mapper/"]
COPY ["src/Database/ROH.Context.Account/ROH.Context.Account.csproj", "src/Database/ROH.Context.Account/"]
COPY ["src/Database/ROH.Context.File/ROH.Context.File.csproj", "src/Database/ROH.Context.File/"]
COPY ["src/Database/ROH.Context.Version/ROH.Context.Version.csproj", "src/Database/ROH.Context.Version/"]
COPY ["src/Common/ROH.StandardModels/ROH.StandardModels.csproj", "src/Common/ROH.StandardModels/"]
COPY ["src/Common/ROH.Utils/ROH.Utils.csproj", "src/Common/ROH.Utils/"]
COPY ["src/Service/ROH.Service.Exception/ROH.Service.Exception.csproj", "src/Service/ROH.Service.Exception/"]
COPY ["src/Database/ROH.Context.Log/ROH.Context.Log.csproj", "src/Database/ROH.Context.Log/"]
COPY ["src/Service/ROH.Service.File/ROH.Service.File.csproj", "src/Service/ROH.Service.File/"]
COPY ["src/Validation/ROH.Validations/ROH.Validations.csproj", "src/Validation/ROH.Validations/"]
RUN dotnet restore "./src/Api/ROH.Api.VersionFiles/ROH.Api.VersionFiles.csproj"
COPY . .
WORKDIR "/src/src/Api/ROH.Api.VersionFiles"
RUN dotnet build "./ROH.Api.VersionFiles.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ROH.Api.VersionFiles.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ROH.Api.VersionFiles.dll"]