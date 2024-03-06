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
docker build -t roh.gateway -f ./ROH.Gateway/Dockerfile .
docker run -d --name ROH.Gateway --network $NETWORK_NAME -p 9001:9001 roh.gateway

# Build and run the ROH.Docker.Server Dockerfile
docker build -t roh.blazor.server -f ./ROH.Blazor.Server/Dockerfile .
docker run -d --name ROH.Blazor --network $NETWORK_NAME -p 9010:9010 roh.blazor.server

# Build and run the ROH.Api.VersionFiles Dockerfile
docker build -t roh.api.versionfiles -f ./ROH.Api.VersionFiles/Dockerfile .
docker run -d --name ROH.Api.VersionFiles --network $NETWORK_NAME -v /home/ROH:/app/files roh.api.versionfiles


# Iterate through other projects and build/run their Dockerfiles
for project in $(find . -type f -name "Dockerfile" -not -path "./ROH.Gateway*" -not -path "./ROH.Blazor.Server*" -not -path "./ROH.Api.VersionFiles*"); do
  project_name=$(dirname $project)
  image_name="$(basename $project_name | tr '[:upper:]' '[:lower:]')"  # Ensure lowercase image name
  docker build -t $image_name -f $project .
  docker run -d --name $(basename $project_name) --network $NETWORK_NAME $image_name
done


#
#This script assumes the following:
#
#Each project has its own Dockerfile.
#The Gateway project is named "ROH.Gateway."
#The other projects are in subdirectories and their Dockerfiles are also named "Dockerfile."
#All projects are connected to the same Docker network named "roh_network."
#
#to make this executable (linux) chmod +x build_and_run.sh
#to run ./build_and_run.sh
