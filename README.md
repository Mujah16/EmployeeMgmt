# EMPLOYEE MANAGEMENT

## Overview

This repository contains a multi-component solution with an API, Azure Functions, infrastructure scripts, and shared services. The project is structured as follows:

- **api/**: ASP.NET Core Web API project.
- **function/**: Azure Functions for background processing.
- **infra/**: Infrastructure as code and deployment scripts.
- **services/**: Shared services such as file processing.
- **shared/**: Shared libraries and utilities.

## Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
- [Visual Studio Code](https://code.visualstudio.com/)

## Getting Started

### Clone the repository

```sh
git clone <repository-url>
cd <repository-folder>
```

### Build and Run the API

```sh
cd api
dotnet build
dotnet run
```

### Run with Docker Compose

```sh
docker-compose up --build
```

### Running Azure Functions

```sh
cd function/ProcessFileFunction
func start
```

## Project Structure

```
api/                # ASP.NET Core Web API
function/           # Azure Functions
infra/              # Infrastructure scripts
services/           # Shared services (e.g., FileProcessor)
shared/             # Shared code and utilities
```

## Configuration

- API settings: [`api/appsettings.json`](api/appsettings.json)
- Development settings: [`api/appsettings.Development.json`](api/appsettings.Development.json)
- Azure Pipeline: [`azure-pipeline.yml`](azure-pipeline.yml)
- Docker Compose: [`docker-compose.yml`](docker-compose.yml)

## Useful Scripts

- Provision Azure resources: [`az_resources.sh`](az_resources.sh)

## License

[MIT](LICENSE)