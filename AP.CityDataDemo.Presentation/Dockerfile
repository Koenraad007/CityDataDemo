FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only project files required for restoring dependencies
COPY ["AP.CityDataDemo.Presentation/AP.CityDataDemo.Presentation.csproj", "AP.CityDataDemo.Presentation/"]
COPY ["AP.CityDataDemo.Application/AP.CityDataDemo.Application.csproj", "AP.CityDataDemo.Application/"]
COPY ["AP.CityDataDemo.Infrastructure/AP.CityDataDemo.Infrastructure.csproj", "AP.CityDataDemo.Infrastructure/"]
COPY ["AP.CityDataDemo.Domain/AP.CityDataDemo.Domain.csproj", "AP.CityDataDemo.Domain/"]
COPY ["AP.CityDataDemo.Shared/AP.CityDataDemo.Shared.csproj", "AP.CityDataDemo.Shared/"]
COPY ["AP.CityDataDemo.sln", "./"]

# Restore only using the presentation project (solution is present for context)
RUN dotnet restore "AP.CityDataDemo.Presentation/AP.CityDataDemo.Presentation.csproj"

# Copy the rest of the source and publish
COPY . .
WORKDIR /src/AP.CityDataDemo.Presentation
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5265

ENTRYPOINT ["dotnet", "AP.CityDataDemo.Presentation.dll"]