# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Use ASP.NET base image for the runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the SDK image for the build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project files and restore dependencies
COPY ["src/Api/ROH.Api.Login/ROH.Api.Login.csproj", "src/Api/ROH.Api.Login/"]
COPY ["src/Common/ROH.Utils/ROH.Utils.csproj", "src/Common/ROH.Utils/"]
COPY ["src/Common/ROH.StandardModels/ROH.StandardModels.csproj", "src/Common/ROH.StandardModels/"]
RUN dotnet restore "src/Api/ROH.Api.Login/ROH.Api.Login.csproj"

# Copy the entire source code and build the project
COPY . .
WORKDIR "/src/Api/ROH.Api.Login"
RUN dotnet build "ROH.Api.Login.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the project to the publish directory
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ROH.Api.Login.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Use the base image to create the final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ROH.Api.Login.dll"]
