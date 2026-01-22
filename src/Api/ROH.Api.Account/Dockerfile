# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 9102:9102


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Api/ROH.Api.Account/ROH.Api.Account.csproj", "src/Api/ROH.Api.Account/"]
COPY ["src/Database/ROH.Context.Account/ROH.Context.Account.csproj", "src/Database/ROH.Context.Account/"]
COPY ["src/Service/ROH.Service.Account/ROH.Service.Account.csproj", "src/Service/ROH.Service.Account/"]
COPY ["src/Common/ROH.Mapper/ROH.Mapping.csproj", "src/Common/ROH.Mapper/"]
COPY ["src/Database/ROH.Context.File/ROH.Context.File.csproj", "src/Database/ROH.Context.File/"]
COPY ["src/Database/ROH.Context.Version/ROH.Context.Version.csproj", "src/Database/ROH.Context.Version/"]
COPY ["src/Common/ROH.StandardModels/ROH.StandardModels.csproj", "src/Common/ROH.StandardModels/"]
COPY ["src/Common/ROH.Utils/ROH.Utils.csproj", "src/Common/ROH.Utils/"]
COPY ["src/Validation/ROH.Validations/ROH.Validations.csproj", "src/Validation/ROH.Validations/"]
COPY ["src/Service/ROH.Service.Authentication/ROH.Service.Authentication.csproj", "src/Service/ROH.Service.Authentication/"]
COPY ["src/Service/ROH.Service.Exception/ROH.Service.Exception.csproj", "src/Service/ROH.Service.Exception/"]
COPY ["src/Database/ROH.Context.Log/ROH.Context.Log.csproj", "src/Database/ROH.Context.Log/"]
RUN dotnet restore "./src/Api/ROH.Api.Account/ROH.Api.Account.csproj"
COPY . .
WORKDIR "/src/src/Api/ROH.Api.Account"
RUN dotnet build "./ROH.Api.Account.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ROH.Api.Account.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ROH.Api.Account.dll"]