using AP.CityDataDemo.Presentation.Components;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Infrastructure.Repositories;
using AP.CityDataDemo.Infrastructure.Data;
using AP.CityDataDemo.Infrastructure.UOW;
using FluentValidation;
using AP.CityDataDemo.Application.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add API controllers
builder.Services.AddControllers();

// Add Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AP.CityDataDemo.Application.CQRS.Queries.Cities.GetAllCitiesQuery>());

// Add data store as singleton
builder.Services.AddSingleton<IInMemoryDataStore, InMemoryDataStore>();

// Add seeder
builder.Services.AddScoped<IDataSeeder, DataSeeder>();


// Add Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add generic repository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Add repositories
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<AddCityDtoValidator>();

var app = builder.Build();

// Seed data on startup
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    await seeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // Enable Swagger only in development
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

// Map API controllers
app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
