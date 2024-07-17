# Use the official .NET SDK 8.0 image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution and project files
COPY d424-software-engineering-capstone.sln ./
COPY WestoverLaneReserve/WestoverLaneReserve.csproj WestoverLaneReserve/
COPY WestoverLaneReserve.Tests/WestoverLaneReserve.Tests.csproj WestoverLaneReserve.Tests/

# Restore the dependencies
RUN dotnet restore

# Copy the rest of the files and build the project
COPY . .
WORKDIR /src/WestoverLaneReserve
RUN dotnet build -c Release -o /app/build

# Publish the project
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Use the official ASP.NET Core runtime 8.0 image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Specify the entry point for the application
ENTRYPOINT ["dotnet", "WestoverLaneReserve.dll"]
