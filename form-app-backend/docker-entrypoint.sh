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

# Function to create database and table
setup_database() {
    echo "Setting up database..."
    
    # Create database - use master database first
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
    
    # Wait a moment for database to be fully available
    sleep 5
    
    # Create table in StudentFormDb
    /opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P "$SA_PASSWORD" -d master -Q "
    USE [StudentFormDb];
    
    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StudentForm]') AND type in (N'U'))
    BEGIN
        CREATE TABLE [dbo].[StudentForm](
            [Id] [int] IDENTITY(1,1) NOT NULL,
            [Name] [nvarchar](100) NOT NULL,
            [Surname] [nvarchar](100) NOT NULL,
            [Faculty] [nvarchar](200) NOT NULL,
            [Motivation] [nvarchar](max) NOT NULL,
            [SubmissionDate] [datetime2](7) NOT NULL,
            CONSTRAINT [PK_StudentForm] PRIMARY KEY CLUSTERED ([Id] ASC)
        );
        PRINT 'StudentForm table created successfully.';
    END
    ELSE
    BEGIN
        PRINT 'StudentForm table already exists.';
    END"
    
    echo "Database setup completed."
}

# Only run this script if the command is "dotnet"
if [ "$1" = "dotnet" ]; then
    # Wait for SQL Server to be ready
    wait_for_sqlserver
    
    # Set up database and table
    setup_database
    
    echo "Database is ready. Starting the application..."
fi

# Execute the original command
exec "$@"