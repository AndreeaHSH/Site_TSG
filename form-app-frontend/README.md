# Student Form Application - Frontend

## Functionality
Angular 19 frontend that provides a form submission interface for students and an admin dashboard for managing submissions. Handles form validation, PDF downloads, and CRUD operations through the backend API.

## Requirements
- Windows 11
- Docker Desktop installed and running
- Git (optional, for cloning repository)

## Startup
For running the complete application (frontend, backend, and database):
1. Please refer to the instructions in the `form-app-backend` repository
2. The unified docker-compose file will build and start all services

## Local Development (Optional)
If you want to run only the frontend for development:
1. Install Node.js 18+ and npm
2. Navigate to the frontend directory
3. Run `npm install`
4. Run `ng serve`
5. Access the development server at http://localhost:4200

Note: The backend API must be running at http://localhost:5000 for the frontend to work properly in development mode.

## Features
- Student form submission with validation
- PDF generation and download
- Admin interface for viewing all submissions
- Form detail, edit, and delete functionality
- Responsive Bootstrap design