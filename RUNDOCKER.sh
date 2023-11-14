#!/bin/bash

# Build and run script for Dockerized projects in a solution

# Define the common Docker network name
NETWORK_NAME="roh_network"

# Build and run the Gateway Dockerfile
docker build -t roh.gateway -f ROH.Gateway/Dockerfile .
docker run -d --name gateway --network=$NETWORK_NAME -p 8080:80 roh.gateway

# Iterate through other projects and build/run their Dockerfiles
for project in $(find . -type f -name "Dockerfile" -not -path "./ROH.Gateway*"); do
  project_name=$(dirname $project)
  docker build -t roh.$project_name -f $project .
  docker run -d --name $project_name --network=$NETWORK_NAME roh.$project_name
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
