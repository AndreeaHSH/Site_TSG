set -e


wait_for_sqlserver() {
    echo "Waiting for SQL Server to be ready..."
    until /opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P "$SA_PASSWORD" -Q "SELECT 1" &> /dev/null; do
        echo "SQL Server is starting up. Waiting..."
        sleep 5
    done
    echo "SQL Server is up and running."
    
    echo "Giving SQL Server time to stabilize..."
    sleep 10
}

setup_database() {
    echo "Setting up database..."
    
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

if [ "$1" = "dotnet" ]; then
    wait_for_sqlserver
    
    setup_database
    
    echo "Database is ready. Starting the application..."
fi

exec "$@"