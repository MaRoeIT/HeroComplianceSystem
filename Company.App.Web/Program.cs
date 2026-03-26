using Company.App.Application.Interfaces;
using Company.App.Application.UseCases.DataExtraction;
using Company.App.Application.UseCases.DataMapping;
using Company.App.Application.UseCases.DataMapping.Models;
using Company.App.Application.UseCases.DataMapping.PurchaseOrder;
using Company.App.Application.UseCases.DetectBatman;
using Company.App.Domain.Specification;
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

// Decides Document type based on pdf header content.
builder.Services.AddScoped<IsOrderDocumentSpec>();
builder.Services.AddScoped<IDocumentTypeDecider, DocumentTypeDecider>();
builder.Services.AddScoped<IDataMapperRouter, DataMapperRouter>();
builder.Services.AddScoped<IDocumentMapper, PurchaseOrderDocumentMapper>();
builder.Services.AddScoped<IDocumentMapper, MaterialDocumentationPackageDocumentMapper>();
builder.Services.AddScoped<IDocumentMapper, AdministrativeRequirementsDocumentMapper>();

// Routes Document type toward the assigned DocumentType by Enum
//builder.Services.AddScoped<IDataMapperRouter, DataMapperRouter>();

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
