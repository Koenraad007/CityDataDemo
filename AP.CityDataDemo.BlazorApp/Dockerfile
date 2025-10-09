# Build Blazor app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["AP.CityDataDemo.BlazorApp/AP.CityDataDemo.BlazorApp.csproj", "AP.CityDataDemo.BlazorApp/"]
COPY ["AP.CityDataDemo.Shared/AP.CityDataDemo.Shared.csproj", "AP.CityDataDemo.Shared/"]
RUN dotnet restore "AP.CityDataDemo.BlazorApp/AP.CityDataDemo.BlazorApp.csproj"

COPY . .
WORKDIR "/src/AP.CityDataDemo.BlazorApp"
RUN dotnet publish -c Release -o /app/publish --no-restore

# Run as dotnet app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "AP.CityDataDemo.BlazorApp.dll"]
