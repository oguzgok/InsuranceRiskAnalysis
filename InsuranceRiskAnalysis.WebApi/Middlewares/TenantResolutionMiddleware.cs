using InsuranceRiskAnalysis.Core.Interfaces;
using InsuranceRiskAnalysis.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InsuranceRiskAnalysis.WebApi.Middlewares
{
    public class TenantResolutionMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantResolutionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            // DI Scope'u manuel açıyoruz çünkü Middleware singleton olabilir ama servisler Scoped.
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var tenantService = scope.ServiceProvider.GetRequiredService<ITenantService>();

                // Header'dan API Key'i oku
                if (context.Request.Headers.TryGetValue("X-ApiKey", out var extractedApiKey))
                {
                    // DB'den bu key kime ait bul (Senior not: Burası normalde Redis/Cache olmalı)
                    // Global Query Filter'ı aşmak için IgnoreQueryFilters kullanıyoruz, 
                    // çünkü henüz TenantId'yi bilmiyoruz!
                    var partner = await dbContext.Partners
                        .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(p => p.ApiKey == extractedApiKey.ToString());

                    if (partner != null)
                    {
                        // Bulduk! Servise set et. Artık DbContext bu ID'yi kullanacak.
                        tenantService.SetTenantId(partner.TenantId);
                    }
                }
            }

            await _next(context);
        }
    }
}
