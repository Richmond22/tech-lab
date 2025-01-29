# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["src/output-cache.csproj", "src/"]
RUN dotnet restore "src/output-cache.csproj"

# Copy the rest of the code
COPY ["src/.", "src/"]

# Build the application
RUN dotnet build "src/output-cache.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "src/output-cache.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy published files from publish stage
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "output-cache.dll"] 