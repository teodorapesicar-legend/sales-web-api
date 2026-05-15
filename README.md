# Sales API

ASP.NET Core Web API deployed on Azure Container Apps.

## Tech Stack
- .NET 10
- Docker
- Azure Container Registry
- Azure Container Apps

## Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /products | Get all products |
| GET | /products/{id} | Get product by ID |
| POST | /products | Create product |
| PUT | /products/{id} | Update product |
| DELETE | /products/{id} | Delete product |

## Deployment

Docker image is hosted on Azure Container Registry and deployed via Azure Container Apps.