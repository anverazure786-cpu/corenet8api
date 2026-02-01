# Use the official .NET Core runtime for ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["Rest API Project.csproj", "./"]
RUN dotnet restore "./Rest API Project.csproj"

# Copy the rest of the application files and build
COPY . .
WORKDIR "/src/"
RUN dotnet build "Rest API Project.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Rest API Project.csproj" -c Release -o /app/publish

# Configure the runtime environment
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rest API Project.dll"]
