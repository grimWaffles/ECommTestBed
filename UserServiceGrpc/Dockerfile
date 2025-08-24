# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy all source files and build the project
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published app from build stage
COPY --from=build /app/publish .

# gRPC services run over HTTP/2
# We'll set it to listen on port 5001 inside the container
EXPOSE 5001

# Configure ASP.NET Core Kestrel to use port 5001
ENV ASPNETCORE_URLS=http://+:5001

ENTRYPOINT ["dotnet", "UserServiceGrpc.dll"]
