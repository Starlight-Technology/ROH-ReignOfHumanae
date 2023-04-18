#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ROH.Api.Login/ROH.Api.Login.csproj", "ROH.Api.Login/"]
RUN dotnet restore "ROH.Api.Login/ROH.Api.Login.csproj"
COPY . .
WORKDIR "/src/ROH.Api.Login"
RUN dotnet build "ROH.Api.Login.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ROH.Api.Login.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ROH.Api.Login.dll"]