using AP.CityDataDemo.Presentation.Components;
using AP.CityDataDemo.Application.Services;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Infrastructure.Repositories;
using AP.CityDataDemo.Infrastructure.Data;
using AP.CityDataDemo.Infrastructure.UOW;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add data store as singleton
builder.Services.AddSingleton<IInMemoryDataStore, InMemoryDataStore>();

// Add seeder
builder.Services.AddScoped<IDataSeeder, DataSeeder>();

// Add application services
builder.Services.AddScoped<ICityService, CityService>();

// Add Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add generic repository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Add repositories
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();

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

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
