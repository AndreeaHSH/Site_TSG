#!/bin/bash
set -e

# Function to check if SQL Server is ready
wait_for_sqlserver() {
    echo "Waiting for SQL Server to be ready..."
    until /opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P "$SA_PASSWORD" -Q "SELECT 1" &> /dev/null; do
        echo "SQL Server is starting up. Waiting..."
        sleep 5
    done
    echo "SQL Server is up and running."
    
    # Give SQL Server additional time to fully initialize
    echo "Giving SQL Server time to stabilize..."
    sleep 10
}

# Function to create database only (let EF handle tables)
setup_database() {
    echo "Setting up database..."
    
    # Create database only - let Entity Framework handle the tables
    /opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P "$SA_PASSWORD" -d master -Q "
    IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'StudentFormDb')
    BEGIN
        CREATE DATABASE [StudentFormDb];
        PRINT 'Database StudentFormDb created successfully.';
    END
    ELSE
    BEGIN
        PRINT 'Database StudentFormDb already exists.';
    END"
    
    echo "Database setup completed. Entity Framework will handle table creation."
}

# Only run this script if the command is "dotnet"
if [ "$1" = "dotnet" ]; then
    # Wait for SQL Server to be ready
    wait_for_sqlserver
    
    # Set up database only (no tables)
    setup_database
    
    echo "Database is ready. Starting the application..."
fi

# Execute the original command
exec "$@"