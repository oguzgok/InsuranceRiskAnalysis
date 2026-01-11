using InsuranceRiskAnalysis.Core.Interfaces;
using InsuranceRiskAnalysis.Infrastructure.Data;
using InsuranceRiskAnalysis.Infrastructure.Repositories;
using InsuranceRiskAnalysis.Services.AgreementServices;
using InsuranceRiskAnalysis.Services.RiskServices;
using InsuranceRiskAnalysis.WebApi.Hubs;
using InsuranceRiskAnalysis.WebApi.Middlewares;
using InsuranceRiskAnalysis.WebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerGen; // Eklendi: Swagger geniþletmeleri için gerekli

var builder = WebApplication.CreateBuilder(args);

// Tenant Service
builder.Services.AddScoped<ITenantService, CurrentTenantService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Gerekli servis injectleri 
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IAgreementRepository, AgreementRepository>();
builder.Services.AddScoped<IRiskService, RiskService>();
builder.Services.AddScoped<IAgreementService, AgreementService>();


// 3. SignalR Ekleme
builder.Services.AddSignalR();

// Standart Servisler
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS Politikasý (Dashboard UI farklý portta olacaðý için gerekli)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .WithOrigins("http://localhost:5000", "https://localhost:5001") // WebUI adresleri
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()); // SignalR için credential þart
});

var app = builder.Build();

// === SEED DATA BAÞLANGIÇ ===
// Uygulama her baþladýðýnda veritabanýný kontrol et
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        // DbInitializer'ý çaðýr
        await DbInitializer.InitializeAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabaný seed iþlemi sýrasýnda bir hata oluþtu.");
    }
}
// === SEED DATA BÝTÝÞ ===

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");
// Middleware'i pipeline'a ekle (Auth'dan önce olmasý iyi olur)
app.UseMiddleware<TenantResolutionMiddleware>();

app.UseAuthorization();

app.MapControllers();
// SignalR Endpoint'i
app.MapHub<RiskHub>("/riskHub");



app.Run();

