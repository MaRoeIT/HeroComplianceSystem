using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DetectBatman;
using Company.App.Infrastructure.Adapters;
using Microsoft.AspNetCore.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DetectBatmanCommand).Assembly));

builder.Services.AddScoped<IHeroScanner, CsvHeroScanner>();

builder.Services.AddScoped(sp =>
{
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(navigationManager.BaseUri) };
});
builder.Services.AddHttpClient("LocalAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7019");
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddControllers();
builder.Services.AddMudServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
