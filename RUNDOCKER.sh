#!/bin/bash

###############################################
# CONFIGURATION
###############################################

NETWORK_NAME="roh_network"
POSTGRES_USER="postgres"
POSTGRES_PASSWORD="postgres123"

###############################################
# CREATE DOCKER NETWORK
###############################################

docker network inspect $NETWORK_NAME > /dev/null 2>&1 || docker network create $NETWORK_NAME

###############################################
# REMOVE OLD CONTAINERS
###############################################

docker rm -f roh.postgres roh.mongo gateway blazor \
  $(docker ps -a -q --filter="name=ROH.*" --format="{{.Names}}") 2>/dev/null

###############################################
# REMOVE OLD IMAGES
###############################################

docker rmi -f roh.postgres.custom roh.mongo.custom \
  $(docker images -q --filter="reference=roh.*") 2>/dev/null

###############################################
# POSTGRES – DATABASE CREATION
###############################################

INIT_DIR="./postgres-init"
mkdir -p $INIT_DIR

cat > $INIT_DIR/create_databases.sql <<EOF
CREATE DATABASE "ROH.FILE";
CREATE DATABASE "ROH.VERSION";
CREATE DATABASE "ROH.ACCOUNT";
CREATE DATABASE "ROH.LOG";
CREATE DATABASE "ROH.PLAYER";
EOF

cat > $INIT_DIR/Dockerfile <<EOF
FROM postgres:16
ENV POSTGRES_USER=$POSTGRES_USER
ENV POSTGRES_PASSWORD=$POSTGRES_PASSWORD
COPY create_databases.sql /docker-entrypoint-initdb.d/
EOF

docker build -t roh.postgres.custom $INIT_DIR

docker run -d \
  --name roh.postgres \
  --network $NETWORK_NAME \
  -p 5432:5432 \
  -e POSTGRES_USER=$POSTGRES_USER \
  -e POSTGRES_PASSWORD=$POSTGRES_PASSWORD \
  roh.postgres.custom

###############################################
# MONGO – READY TO USE
###############################################

docker run -d \
  --name roh.mongo \
  --network $NETWORK_NAME \
  -p 27017:27017 \
  mongo:7

###############################################
# GATEWAY
###############################################

docker build -t roh.gateway -f ./src/Gateway/ROH.Gateway/Dockerfile .
docker run -d \
  --name ROH.Gateway \
  --network $NETWORK_NAME \
  -p 9001:9001 \
  roh.gateway

###############################################
# BLAZOR SERVER
###############################################

docker build -t roh.blazor.server -f ./src/Site/ROH.Site/Dockerfile .
docker run -d \
  --name ROH.Blazor \
  --network $NETWORK_NAME \
  -p 9010:9010 \
  roh.blazor.server

###############################################
# API VERSIONFILES
###############################################

docker build -t roh.api.versionfiles -f ./src/Api/ROH.Api.VersionFiles/Dockerfile .
docker run -d \
  --name ROH.Api.VersionFiles \
  --network $NETWORK_NAME \
  -p 9100:9100 \
  -v /home/roh:/app/ROH/updateFiles \
  -u roh \
  -e ROH_DATABASE_CONNECTION_STRING_FILE="Host=roh.postgres;Port=5432;Database=ROH.FILE;Username=$POSTGRES_USER;Password=$POSTGRES_PASSWORD;" \
  -e DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_HTTP2UNENCRYPTEDSUPPORT=true \
  roh.api.versionfiles

###############################################
# API VERSION
###############################################

docker build -t roh.api.version -f ./src/Api/ROH.Api.Version/Dockerfile .
docker run -d \
  --name ROH.Api.Version \
  --network $NETWORK_NAME \
  -p 9101:9101 \
  -e ROH_DATABASE_CONNECTION_STRING_VERSION="Host=roh.postgres;Port=5432;Database=ROH.VERSION;Username=$POSTGRES_USER;Password=$POSTGRES_PASSWORD;" \
  -e DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_HTTP2UNENCRYPTEDSUPPORT=true \
  roh.api.version

###############################################
# API ACCOUNT
###############################################

docker build -t roh.api.account -f ./src/Api/ROH.Api.Account/Dockerfile .
docker run -d \
  --name ROH.Api.Account \
  --network $NETWORK_NAME \
  -p 9102:9102 \
  -e ROH_DATABASE_CONNECTION_STRING_ACCOUNT="Host=roh.postgres;Port=5432;Database=ROH.ACCOUNT;Username=$POSTGRES_USER;Password=$POSTGRES_PASSWORD;" \
  -e DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_HTTP2UNENCRYPTEDSUPPORT=true \
  roh.api.account

###############################################
# API LOGIN
###############################################

docker build -t roh.api.login -f ./src/Api/ROH.Api.Login/Dockerfile .
docker run -d \
  --name ROH.Api.Login \
  --network $NETWORK_NAME \
  -p 9103:9103 \
  -e ROH_DATABASE_CONNECTION_STRING_ACCOUNT="Host=roh.postgres;Port=5432;Database=ROH.ACCOUNT;Username=$POSTGRES_USER;Password=$POSTGRES_PASSWORD;" \
  -e DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_HTTP2UNENCRYPTEDSUPPORT=true \
  roh.api.login

###############################################
# API LOG
###############################################

docker build -t roh.api.log -f ./src/Api/ROH.Api.Log/Dockerfile .
docker run -d \
  --name ROH.Api.Log \
  --network $NETWORK_NAME \
  -p 9104:9104 \
  -e ROH_DATABASE_CONNECTION_STRING_LOG="Host=roh.postgres;Port=5432;Database=ROH.LOG;Username=$POSTGRES_USER;Password=$POSTGRES_PASSWORD;" \
  -e DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_HTTP2UNENCRYPTEDSUPPORT=true \
  roh.api.log

###############################################
# API PLAYER
###############################################

docker build -t roh.api.player -f ./src/Api/ROH.Api.Player/Dockerfile .
docker run -d \
  --name ROH.Api.Player \
  --network $NETWORK_NAME \
  -p 9105:9105 \
  -e ROH_DATABASE_CONNECTION_STRING_PLAYER="Host=roh.postgres;Port=5432;Database=ROH.PLAYER;Username=$POSTGRES_USER;Password=$POSTGRES_PASSWORD;" \
  -e DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_HTTP2UNENCRYPTEDSUPPORT=true \
  roh.api.player

###############################################
# API PLAYER SYNC STATE (MONGO)
###############################################

docker build -t roh.api.playersync.state -f ./src/Api/ROH.Api.PlayerSync.State/Dockerfile .
docker run -d \
  --name ROH.Api.PlayerSync.State \
  --network $NETWORK_NAME \
  -p 9210:9210 \
  -e ROH_MONGO_PLAYER_CONNECTION_STRING="mongodb://roh.mongo:27017/?retryWrites=true&loadBalanced=false&serverSelectionTimeoutMS=5000&connectTimeoutMS=10000" \
  -e DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_HTTP2UNENCRYPTEDSUPPORT=true \
  roh.api.playersync.state

###############################################
# WORKER GET NEARBY PLAYER
###############################################

docker build -t roh.worker.getnearbyplayer -f ./src/Worker/ROH.Worker.GetNearbyPlayer/Dockerfile .
docker run -d \
  --name ROH.Worker.GetNearbyPlayer \
  --network $NETWORK_NAME \
  roh.worker.getnearbyplayer

echo "==============================================="
echo " ALL ROH CONTAINERS HAVE BEEN STARTED "
echo " Postgres: localhost:5432"
echo " Mongo: localhost:27017"
echo "==============================================="
