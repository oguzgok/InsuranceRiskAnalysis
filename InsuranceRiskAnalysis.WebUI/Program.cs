using InsuranceRiskAnalysis.Core.Interfaces;
using InsuranceRiskAnalysis.Infrastructure.Data;
using InsuranceRiskAnalysis.Infrastructure.Repositories;
using InsuranceRiskAnalysis.Services.AgreementServices;
using InsuranceRiskAnalysis.Services.RiskServices;
using InsuranceRiskAnalysis.WebApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IAgreementRepository, AgreementRepository>();
builder.Services.AddScoped<IRiskService, RiskService>();
builder.Services.AddScoped<IAgreementService, AgreementService>();

builder.Services.AddScoped<ITenantService, CurrentTenantService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
