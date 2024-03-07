#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN addgroup --gid 1000 roh && adduser --uid 1010 --gid 1000 --disabled-password --gecos '' roh
USER roh
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
VOLUME ["/app/ROH/updateFiles"]


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ROH.Api.VersionFiles/ROH.Api.VersionFiles.csproj", "ROH.Api.VersionFiles/"]
COPY ["ROH.Context/ROH.Context.csproj", "ROH.Context/"]
COPY ["ROH.Domain/ROH.Domain.csproj", "ROH.Domain/"]
COPY ["ROH.Interfaces/ROH.Interfaces.csproj", "ROH.Interfaces/"]
COPY ["ROH.StandardModels/ROH.StandardModels.csproj", "ROH.StandardModels/"]
COPY ["ROH.Mapper/ROH.Mapper.csproj", "ROH.Mapper/"]
COPY ["ROH.Repository/ROH.Repository.csproj", "ROH.Repository/"]
COPY ["ROH.Services/ROH.Services.csproj", "ROH.Services/"]
COPY ["ROH.Validations/ROH.Validations.csproj", "ROH.Validations/"]
COPY ["ROH.Utils/ROH.Utils.csproj", "ROH.Utils/"]
RUN dotnet restore "./ROH.Api.VersionFiles/./ROH.Api.VersionFiles.csproj"
COPY . .
WORKDIR "/src/ROH.Api.VersionFiles"
RUN dotnet build "./ROH.Api.VersionFiles.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ROH.Api.VersionFiles.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ROH.Api.VersionFiles.dll"]