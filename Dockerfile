# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy source
COPY . .

# Restore and publish API project
RUN dotnet restore "03.Layer Api/ChatModularMicroservice.Api/ChatModularMicroservice.Api.csproj"
RUN dotnet publish "03.Layer Api/ChatModularMicroservice.Api/ChatModularMicroservice.Api.csproj" -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "ChatModularMicroservice.Api.dll"]
