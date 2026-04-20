using Company.App.Application.Interfaces;
using Company.App.Application.Interfaces.OneSubSea.AdministrativeRequirements;
using Company.App.Application.UseCases.DataExtraction;
using Company.App.Application.UseCases.DataMapping;
using Company.App.Application.UseCases.DataMapping.OneSubSea;
using Company.App.Application.UseCases.DataMapping.OneSubSea.AdministrativeRequirementsDocument;
using Company.App.Application.UseCases.DataMapping.OneSubSea.PurchaseOrderDocument;
using Company.App.Domain.Entities.OneSubSea;
using Company.App.Domain.Specification;
using Company.App.Infrastructure.Adapters;
using Company.App.Infrastructure.Persistence;
using Company.App.Infrastructure.Persistence.Repositories;
using Company.App.Web;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ExtractPdfCommand).Assembly));

builder.Services.AddScoped<IPdfDataExtractor, PdfPigDataExtractor>();
builder.Services.AddScoped<IHeroScanner, CsvHeroScanner>();
builder.Services.AddScoped<IHeroRepository, HeroRepository>();

builder.Services.AddHttpClient("PdfExtractAPI");

builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"/home/app/.aspnet/DataProtection-keys"));

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddMudServices();

// Decides Document type based on pdf header content.
builder.Services.AddScoped<IsOrderDocumentSpec>();
builder.Services.AddScoped<IDocumentTypeDecider, DocumentTypeDecider>();
builder.Services.AddScoped<IDataMapperRouter, DataMapperRouter>();
builder.Services.AddScoped<IDocumentMapper, MapPurchaseOrderDocument>();
builder.Services.AddScoped<IDocumentMapper, MapMaterialDocumentationPackageDocument>();
builder.Services.AddScoped<IDocumentMapper, MapAdministrativeRequirementsDocument>();

// --- Core services ---
builder.Services.AddScoped<IPdfDataExtractor, PdfPigDataExtractor>(); // :contentReference[oaicite:0]{index=0}
builder.Services.AddScoped<IHeroScanner, CsvHeroScanner>();           // :contentReference[oaicite:1]{index=1}
builder.Services.AddScoped<IHeroRepository, HeroRepository>();        // :contentReference[oaicite:2]{index=2}

// --- Document type detection ---
builder.Services.AddScoped<IsOrderDocumentSpec>();
builder.Services.AddScoped<IDocumentTypeDecider, DocumentTypeDecider>(); // :contentReference[oaicite:3]{index=3}

// --- Routing ---
builder.Services.AddScoped<IDataMapperRouter, DataMapperRouter>();       // :contentReference[oaicite:4]{index=4}

// --- Document mappers ---
builder.Services.AddScoped<IDocumentMapper, MapPurchaseOrderDocument>(); // :contentReference[oaicite:5]{index=5}
builder.Services.AddScoped<IDocumentMapper, MapMaterialDocumentationPackageDocument>();
builder.Services.AddScoped<IDocumentMapper, MapAdministrativeRequirementsDocument>();

// --- Purchase Order specific mappers ---
builder.Services.AddScoped<IPurchaseOrderHeaderMapper, MapHeader>();     // :contentReference[oaicite:6]{index=6}
builder.Services.AddScoped<IPurchaseOrderOverheadMapper, MapPurchaseOrderOverhead>(); // :contentReference[oaicite:7]{index=7}
builder.Services.AddScoped<IItemMapper, MapItems>();                                      // :contentReference[oaicite:8]{index=8}
builder.Services.AddScoped<IMaterialDescriptionMapper, MapMaterialDescription>();         // :contentReference[oaicite:9]{index=9}

// --- Administrative Requirement specific mappers ---
builder.Services.AddScoped<IAdministrativeRequirementsHeaderMapper, MapAdministrativeRequirementsHeader>();
builder.Services.AddScoped<IAppendicesHeaderMapperRouter, AppendicesHeaderMapperRouter>();

builder.Services.AddScoped<IAppendicesHeaderMapper, MapAppendicesHeader>();
builder.Services.AddScoped<IAppendicesHeaderMapperRouter, AppendicesHeaderMapperRouter>();

builder.Services.AddScoped<IDocumentMapper, MapHsseRequirementsForSuppliers>();
builder.Services.AddScoped<IDocumentMapper, MapSupplierFinalInspectionSpecification>();
builder.Services.AddScoped<IDocumentMapper, MapSupplierDocumentationSpecification>();
builder.Services.AddScoped<IDocumentMapper, MapSupplierPackingMarkingandShippingInstruction>();
builder.Services.AddScoped<IDocumentMapper, MapTraceabilitySpecificationForSuppliers>();
builder.Services.AddScoped<IDocumentMapper, MapSupplierDocumentationSpecification>();

// --- Party mappers ---
builder.Services.AddScoped<ISellerMapper, MapSeller>(); // :contentReference[oaicite:10]{index=10}
builder.Services.AddScoped<IBuyerMapper, MapBuyer>();   // :contentReference[oaicite:11]{index=11}

// --- Address mappers (GENERIC ⚠️)
builder.Services.AddScoped<IVendorAddressMapper, MapVendorAddress>(); // :contentReference[oaicite:12]{index=12}
builder.Services.AddScoped<IInvoiceAddressMapper, MapInvoiceAddress>();
builder.Services.AddScoped<IAddressMapper<DeliveryAddress>, MapDeliveryAddress>();

// --- Basic data text mapper ---
builder.Services.AddScoped<IBasicDataTextMapper, MapBasicDataText>(); // :contentReference[oaicite:13]{index=13}

// Routes Document type toward the assigned DocumentType by Enum
//builder.Services.AddScoped<IDataMapperRouter, DataMapperRouter>();
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
