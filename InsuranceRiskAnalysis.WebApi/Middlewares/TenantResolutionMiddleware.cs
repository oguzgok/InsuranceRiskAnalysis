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

        public async Task InvokeAsync(HttpContext context)
        {
            // Ana RequestServices içindeki AppDbContext'i sakın çağırma!
            // Eğer çağırırsan, TenantId henüz set edilmediği için "boş" bir DbContext oluşur 
            // ve Request boyunca o boş nesne kullanılır.

            // Bunun yerine sadece API Key kontrolü için geçici (kullan-at) bir scope açıyoruz.
            using (var scope = context.RequestServices.CreateScope())
            {
                var tempDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Header'dan API Key'i oku
                if (context.Request.Headers.TryGetValue("X-ApiKey", out var extractedApiKey))
                {
                    // Geçici DbContext ile Partner'i bul
                    var partner = await tempDbContext.Partners
                        .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(p => p.ApiKey == extractedApiKey.ToString());

                    if (partner != null)
                    {
                        // BULDUK! Şimdi ANA (Main) Request scope'undaki TenantService'e yazıyoruz.
                        // Dikkat: Burada 'context.RequestServices' kullanıyoruz, 'scope' değil.
                        var mainTenantService = context.RequestServices.GetRequiredService<ITenantService>();
                        mainTenantService.SetTenantId(partner.TenantId);
                    }
                }
            } // Scope burada ölür, tempDbContext yok olur.

            // Artık Controller'a gidildiğinde, sistem AppDbContext'i İLK KEZ isteyecek.
            // TenantService dolu olduğu için, AppDbContext doğru ID ile oluşacak!

            await _next(context);
        }
    }
}
