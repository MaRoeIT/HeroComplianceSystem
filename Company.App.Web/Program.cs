using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction;
using Company.App.Application.UseCases.DetectBatman;
using Company.App.Infrastructure.Adapters;
using Company.App.Web;
using Microsoft.AspNetCore.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ExtractPdfCommand).Assembly));

builder.Services.AddScoped<IPdfDataExtractor, PdfPigDataExtractor>();
builder.Services.AddScoped<IHeroScanner, CsvHeroScanner>();

builder.Services.AddHttpClient("PdfExtractAPI");

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
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

app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseAntiforgery();

app.Run();
