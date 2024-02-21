# Define the common Docker network name
$NETWORK_NAME = "roh_network"

# Check if the Docker network exists, and create it if it doesn't
docker network inspect $NETWORK_NAME > $null 2>&1
if ($LASTEXITCODE -ne 0) { docker network create $NETWORK_NAME }

# Remove existing containers with names 'gateway', 'blazor', and other project containers
docker rm -f gateway blazor @(docker ps -a -q --filter="name=ROH.*" --format="{{.Names}}") 2>$null

# Remove existing images with names starting with 'roh.'
docker rmi -f @(docker images -q --filter="reference=roh.*") 2>$null

# Build and run the Gateway Dockerfile
docker build -t roh.gateway -f ./ROH.Gateway/Dockerfile .
docker run -d --name gateway --network $NETWORK_NAME -p 9001:9001 roh.gateway

# Build and run the ROH.Docker.Server Dockerfile
docker build -t roh.blazor.server -f ./ROH.Blazor.Server/Dockerfile .
docker run -d --name blazor --network $NETWORK_NAME -p 9010:9010 roh.blazor.server

# Iterate through other projects and build/run their Dockerfiles
$projects = Get-ChildItem -Path . -Filter Dockerfile -Recurse | Where-Object { $_.FullName -notlike "*ROH.Gateway*" -and $_.FullName -notlike "*ROH.Blazor.Server*" }
foreach ($project in $projects) {
  $project_name = Split-Path $project -Parent
  $image_name = (Split-Path $project -Leaf).ToLower()
  docker build -t $image_name -f $project.FullName .
  docker run -d --name (Split-Path $project -Leaf) --network $NETWORK_NAME $image_name
}
