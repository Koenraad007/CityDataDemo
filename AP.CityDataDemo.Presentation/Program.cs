using AP.CityDataDemo.Application.Extensions;
using AP.CityDataDemo.Infrastructure.Extensions;
using AP.CityDataDemo.Presentation.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterApplication();
builder.Services.RegisterInfrastructure();

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

app.UseErrorHandlingMiddleware();

app.MapControllers();

// Apply any pending EF Core migrations on startup so the sqlite DB has the required schema.
try
{
    using var scope = app.Services.CreateAsyncScope();
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AP.CityDataDemo.Infrastructure.Contexts.CityDataDemoContext>();
    await db.Database.MigrateAsync();
    Console.WriteLine("Database migrations applied successfully.");
}
catch (Exception ex)
{
    // If migration fails, we still attempt to run the app but log the error.
    Console.WriteLine($"An error occurred while migrating the database: {ex}");
}

await app.RunAsync();
