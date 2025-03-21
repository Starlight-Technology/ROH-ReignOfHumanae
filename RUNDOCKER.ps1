# Define the common Docker network name
$NETWORK_NAME = "roh_network"

# Check if the Docker network exists, and create it if it doesn't
if (-not (docker network inspect $NETWORK_NAME -ErrorAction SilentlyContinue)) {
    docker network create $NETWORK_NAME
}

# Remove existing containers with names 'gateway', 'blazor', and other project containers
docker rm -f gateway, blazor -ErrorAction SilentlyContinue
docker ps -a -q --filter "name=ROH.*" | ForEach-Object { docker rm -f $_ }

# Remove existing images with names starting with 'roh.'
docker images -q --filter "reference=roh.*" | ForEach-Object { docker rmi -f $_ }

# Build and run the Gateway Dockerfile
docker build -t roh.gateway -f ./src/Gateway/ROH.Gateway/Dockerfile .
docker run -d --name ROH.Gateway --network $NETWORK_NAME -p 9001:9001 roh.gateway

# Build and run the ROH.Blazor.Server Dockerfile
docker build -t roh.blazor.server -f ./src/Blazor/ROH.Blazor.Server/Dockerfile .
docker run -d --name ROH.Blazor --network $NETWORK_NAME -p 9010:9010 roh.blazor.server

# Build and run the ROH.Api.VersionFiles Dockerfile
docker build -t roh.api.versionfiles -f ./src/Api/ROH.Api.VersionFiles/Dockerfile .
docker run -d --name ROH.Api.VersionFiles --network $NETWORK_NAME -v C:\roh:/app/ROH/updateFiles -e ROH_DATABASE_CONNECTION_STRING_FILE="Host=localhost;Port=5432;Database=ROH.FILE;Username=postgres;Password=123;" roh.api.versionfiles

# Build and run the ROH.Api.Version Dockerfile
docker build -t roh.api.version -f ./src/Api/ROH.Api.Version/Dockerfile .
docker run -d --name ROH.Api.Version --network $NETWORK_NAME -e ROH_DATABASE_CONNECTION_STRING_VERSION="Host=localhost;Port=5432;Database=ROH.VERSION;Username=postgres;Password=123;" roh.api.version

# Build and run the ROH.Api.Account Dockerfile
docker build -t roh.api.account -f ./src/Api/ROH.Api.Account/Dockerfile .
docker run -d --name ROH.Api.Account --network $NETWORK_NAME -e ROH_DATABASE_CONNECTION_STRING_ACCOUNT="Host=localhost;Port=5432;Database=ROH.ACCOUNT;Username=postgres;Password=123;" roh.api.account

# Build and run the ROH.Api.Login Dockerfile
docker build -t roh.api.login -f ./src/Api/ROH.Api.Login/Dockerfile .
docker run -d --name ROH.Api.Login --network $NETWORK_NAME -e ROH_DATABASE_CONNECTION_STRING_ACCOUNT="Host=localhost;Port=5432;Database=ROH.ACCOUNT;Username=postgres;Password=123;" roh.api.login

# Build and run the ROH.Api.Log Dockerfile
docker build -t roh.api.log -f ./src/Api/ROH.Api.Log/Dockerfile .
docker run -d --name ROH.Api.Log --network $NETWORK_NAME -e ROH_DATABASE_CONNECTION_STRING_LOG="Host=localhost;Port=5432;Database=ROH.LOG;Username=postgres;Password=123;" roh.api.log