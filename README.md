# Sales API

ASP.NET Core Web API deployed on Azure Container Apps with Azure SQL Database and Key Vault secrets management.

## Tech Stack
- .NET 10
- Docker
- Azure Container Registry
- Azure Container Apps
- Azure SQL Database
- Azure Key Vault
- Managed Identity

## Architecture

Local code → Docker image → Azure Container Registry → Azure Container Apps
                                                              ↓
                                                      Azure SQL Database
                                                      Azure Key Vault (connection string)

## Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /products | Get all products |
| GET | /products/{id} | Get product by ID |
| POST | /products | Create product |
| PUT | /products/{id} | Update product |
| DELETE | /products/{id} | Delete product |

## Deployment

Docker image is hosted on Azure Container Registry and deployed via Azure Container Apps. Connection string is stored in Azure Key Vault and accessed via Managed Identity — no credentials in code or config files.

## Screenshots

**Live App**
![App](docs/screenshots/app.png)

**Azure Container Registry**
![ACR](docs/screenshots/acr-repository-v3.png)

**Container App Overview**
![Container App](docs/screenshots/container-app-overview.png)

**Container Registry**
![Registry](docs/screenshots/container-registry.png)

**Key Vault Secrets**
![Key Vault](docs/screenshots/key-vault-secrets.png)

**Azure SQL Database**
![SQL](docs/screenshots/sql-database-overview.png)