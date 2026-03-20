using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DetectBatman;
using Company.App.Infrastructure.Adapters;
using Company.App.Infrastructure.Persistence;
using Company.App.Infrastructure.Persistence.Repositories;
using Company.App.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DetectBatmanCommand).Assembly));

builder.Services.AddScoped<IHeroScanner, CsvHeroScanner>();
builder.Services.AddScoped<IHeroRepository, HeroRepository>();

builder.Services.AddHttpClient("HeroAPI");

builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"/home/app/.aspnet/DataProtection-keys"));

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddMudServices();

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));



var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (app.Environment.IsDevelopment())
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }
    else
    {
        db.Database.Migrate();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
