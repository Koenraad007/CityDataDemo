FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj(s) and restore as distinct layers
# COPY ["AP.CityDataDemo.Presentation/AP.CityDataDemo.Presentation.csproj", "AP.CityDataDemo.Presentation/"]
# COPY ["AP.CityDataDemo.Application/AP.CityDataDemo.Application.csproj", "AP.CityDataDemo.Application/"]
# COPY ["AP.CityDataDemo.Infrastructure/AP.CityDataDemo.Infrastructure.csproj", "AP.CityDataDemo.Infrastructure/"]
# COPY ["AP.CityDataDemo.Domain/AP.CityDataDemo.Domain.csproj", "AP.CityDataDemo.Domain/"]
# COPY ["AP.CityDataDemo.Shared/AP.CityDataDemo.Shared.csproj", "AP.CityDataDemo.Shared/"]
COPY . .
# Copy the solution and nuget.config (if you have one) so restore can see solution-level
# settings and private feed configuration before running restore. Restoring the
# solution helps when package sources or central package management are defined
# at the solution or repo root.
# COPY ["AP.CityDataDemo.sln", "./"]

# Restore the full solution (will use nuget.config if present)
RUN dotnet restore "AP.CityDataDemo.sln"

# copy everything else and publish
COPY . .
WORKDIR /src/AP.CityDataDemo.Presentation
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Listen on port 80 inside the container
# ENV ASPNETCORE_URLS=http://+:80
EXPOSE 5265

ENTRYPOINT ["dotnet", "AP.CityDataDemo.Presentation.dll"]