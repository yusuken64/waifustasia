# Use the .NET Core SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

# Copy the .csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining source code
COPY . ./

# Build the app
RUN dotnet publish -c Release -o out

# Create the runtime image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Expose the port the app runs on
EXPOSE 80

# Start the app
ENTRYPOINT ["dotnet", "Waifustasia.dll"]
