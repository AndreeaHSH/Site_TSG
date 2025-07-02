# Student Form Application - Backend

## Functionality
ASP.NET Core 7 backend API that allows students to submit forms and generates PDF documents. Includes admin functionality for managing student submissions with full CRUD operations.

## Requirements
- Docker Desktop installed and running
- Git (optional, for cloning repository)

## Startup
1. Ensure Docker Desktop is running
2. Place the `docker-compose.yml` file in the parent directory:
```
|Parent Directory
|--form-app-backend
|--form-app-frontend
|--docker-compose.yml
```
3. Open Command Prompt or PowerShell in the parent directory
4. Run: `docker-compose up -d --build`
5. Access the application at http://localhost:4200
6. Access API documentation at http://localhost:5000/swagger

## API Endpoints
- `POST /api/forms` - Submit student form (returns PDF)
- `GET /api/forms` - Get all forms
- `GET /api/forms/{id}` - Get specific form
- `PUT /api/forms/{id}` - Update form
- `DELETE /api/forms/{id}` - Delete form