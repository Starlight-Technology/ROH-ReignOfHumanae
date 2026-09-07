FROM mcr.microsoft.com/devcontainers/dotnet:10.0

USER root

RUN apt-get update && apt-get install -y --no-install-recommends \
    git \
    curl \
    wget \
    unzip \
    jq \
    ca-certificates \
    gnupg \
    postgresql-client \
    && rm -rf /var/lib/apt/lists/*

RUN curl -fsSL https://www.mongodb.org/static/pgp/server-8.0.asc \
    | gpg --dearmor -o /usr/share/keyrings/mongodb-server-8.0.gpg \
    && echo "deb [ signed-by=/usr/share/keyrings/mongodb-server-8.0.gpg ] https://repo.mongodb.org/apt/debian bookworm/mongodb-org/8.0 main" \
    > /etc/apt/sources.list.d/mongodb-org-8.0.list \
    && apt-get update \
    && apt-get install -y --no-install-recommends mongodb-mongosh \
    && rm -rf /var/lib/apt/lists/*

USER vscode

RUN dotnet tool install --global dotnet-ef \
    && dotnet tool install --global dotnet-aspnet-codegenerator

WORKDIR /workspaces