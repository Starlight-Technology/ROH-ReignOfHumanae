#!/bin/bash

# Define the common Docker network name
NETWORK_NAME="roh_network"

# Check if the Docker network exists, and create it if it doesn't
docker network inspect $NETWORK_NAME > /dev/null 2>&1 || docker network create $NETWORK_NAME

# Remove existing containers with names 'gateway', 'blazor', and other project containers
docker rm -f gateway blazor $(docker ps -a -q --filter="name=ROH.*" --format="{{.Names}}") 2>/dev/null

# Remove existing images with names starting with 'roh.'
docker rmi -f $(docker images -q --filter="reference=roh.*") 2>/dev/null

# Build and run the Gateway Dockerfile
docker build -t roh.gateway -f ./src/Gateway/ROH.Gateway/Dockerfile .
docker run -d \
  --name ROH.Gateway \
  --network $NETWORK_NAME \
  -p 9001:9001 \
  roh.gateway

# Build and run the ROH.Docker.Server Dockerfile
docker build -t roh.blazor.server -f ./src/Blazor/ROH.Blazor.Server/Dockerfile .
docker run -d \
  --name ROH.Blazor \
  --network $NETWORK_NAME \
  -p 9010:9010 \
  roh.blazor.server

# Build and run the ROH.Api.VersionFiles Dockerfile
docker build -t roh.api.versionfiles -f ./src/Api/ROH.Api.VersionFiles/Dockerfile .
docker run -d \
  --name ROH.Api.VersionFiles \
  --network $NETWORK_NAME \
  -p 9100:9100 \
  -v /home/roh:/app/ROH/updateFiles \
  -u roh \
  -e ROH_DATABASE_CONNECTION_STRING_FILE="Host=192.168.0.65;Port=5432;Database=ROH.FILE;Username=postgres;Password=postgres123;" \
  -e DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_HTTP2UNENCRYPTEDSUPPORT=true \
  roh.api.versionfiles

# Build and run the ROH.Api.Version Dockerfile
docker build -t roh.api.version -f ./src/Api/ROH.Api.Version/Dockerfile .
docker run -d \
  --name ROH.Api.Version \
  --network $NETWORK_NAME \
  -p 9101:9101 \
  -e ROH_DATABASE_CONNECTION_STRING_VERSION="Host=192.168.0.65;Port=5432;Database=ROH.VERSION;Username=postgres;Password=postgres123;" \
  -e DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_HTTP2UNENCRYPTEDSUPPORT=true \
  roh.api.version

# Build and run the ROH.Api.Account Dockerfile
docker build -t roh.api.account -f ./src/Api/ROH.Api.Account/Dockerfile .
docker run -d \
  --name ROH.Api.Account \
  --network $NETWORK_NAME \
  -p 9102:9102 \
  -e ROH_DATABASE_CONNECTION_STRING_ACCOUNT="Host=192.168.0.65;Port=5432;Database=ROH.ACCOUNT;Username=postgres;Password=postgres123;" \
  -e DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_HTTP2UNENCRYPTEDSUPPORT=true \
  roh.api.account

# Build and run the ROH.Api.Login Dockerfile
docker build -t roh.api.login -f ./src/Api/ROH.Api.Login/Dockerfile .
docker run -d \
  --name ROH.Api.Login \
  --network $NETWORK_NAME \
  -p 9103:9103 \
  -e ROH_DATABASE_CONNECTION_STRING_ACCOUNT="Host=192.168.0.65;Port=5432;Database=ROH.ACCOUNT;Username=postgres;Password=postgres123;" \
  -e DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_HTTP2UNENCRYPTEDSUPPORT=true \
  roh.api.login

# Build and run the ROH.Api.Log Dockerfile
docker build -t roh.api.log -f ./src/Api/ROH.Api.Log/Dockerfile .
docker run -d \
  --name ROH.Api.Log \
  --network $NETWORK_NAME \
  -p 9104:9104 \
  -e ROH_DATABASE_CONNECTION_STRING_LOG="Host=192.168.0.65;Port=5432;Database=ROH.LOG;Username=postgres;Password=postgres123;" \
  -e DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_HTTP2UNENCRYPTEDSUPPORT=true \
  roh.api.log