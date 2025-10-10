using AP.CityDataDemo.Application.Extensions;
using AP.CityDataDemo.Infrastructure.Extensions;
using AP.CityDataDemo.Presentation.Extensions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/keys"))
    .SetApplicationName("CityDataDemoApp");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterApplication();
builder.Services.RegisterInfrastructure(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Allow cross-origin requests from the Blazor web container (and anywhere for development)
app.UseCors("AllowAll");

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AP.CityDataDemo.Infrastructure.Contexts.CityDataDemoContext>();
    context.Database.Migrate();

    // Seed the database with initial data
    if (!context.Countries.Any() && !context.Cities.Any())
    {
        Console.WriteLine("Seeding initial data...");
        context.Countries.AddRange(
            new AP.CityDataDemo.Domain.Country { Name = "USA" },
            new AP.CityDataDemo.Domain.Country { Name = "Canada" }
        );
        context.SaveChanges();

        // cities
        context.Cities.AddRange(
            new AP.CityDataDemo.Domain.City { Name = "New York", Population = 8419600, CountryId = context.Countries.First(c => c.Name == "USA").Id },
            new AP.CityDataDemo.Domain.City { Name = "Los Angeles", Population = 3980400, CountryId = context.Countries.First(c => c.Name == "USA").Id },
            new AP.CityDataDemo.Domain.City { Name = "Chicago", Population = 2716000, CountryId = context.Countries.First(c => c.Name == "USA").Id },
            new AP.CityDataDemo.Domain.City { Name = "Toronto", Population = 2930000, CountryId = context.Countries.First(c => c.Name == "Canada").Id },
            new AP.CityDataDemo.Domain.City { Name = "Vancouver", Population = 675218, CountryId = context.Countries.First(c => c.Name == "Canada").Id },
            new AP.CityDataDemo.Domain.City { Name = "Montreal", Population = 1780000, CountryId = context.Countries.First(c => c.Name == "Canada").Id }
        );
        context.SaveChanges();
    }
    else
    {
        Console.WriteLine("Database already contains data. Skipping seeding.");
    }
}

app.UseErrorHandlingMiddleware();

app.MapControllers();

// Apply any pending EF Core migrations on startup so the sqlite DB has the required schema.
// try
// {
//     using var scope = app.Services.CreateAsyncScope();
//     var services = scope.ServiceProvider;
//     var db = services.GetRequiredService<AP.CityDataDemo.Infrastructure.Contexts.CityDataDemoContext>();
//     await db.Database.MigrateAsync();
//     Console.WriteLine("Database migrations applied successfully.");
// }
// catch (Exception ex)
// {
//     // If migration fails, we still attempt to run the app but log the error.
//     Console.WriteLine($"An error occurred while migrating the database: {ex}");
// }

await app.RunAsync();
