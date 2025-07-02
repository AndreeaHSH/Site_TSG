FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["form-app-backend.csproj", "./"]
RUN dotnet restore "form-app-backend.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "form-app-backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "form-app-backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final

# Install SQL Server tools for the entrypoint script
RUN apt-get update \
    && apt-get install -y curl gnupg \
    && curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
    && curl https://packages.microsoft.com/config/debian/11/prod.list > /etc/apt/sources.list.d/mssql-release.list \
    && apt-get update \
    && ACCEPT_EULA=Y apt-get install -y mssql-tools unixodbc-dev \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

# Add SQL Server tools to the PATH
ENV PATH="$PATH:/opt/mssql-tools/bin"

WORKDIR /app
COPY --from=publish /app/publish .

# Copy the entrypoint script
COPY docker-entrypoint.sh /usr/local/bin/
RUN chmod +x /usr/local/bin/docker-entrypoint.sh

ENTRYPOINT ["docker-entrypoint.sh"]
CMD ["dotnet", "form-app-backend.dll"]