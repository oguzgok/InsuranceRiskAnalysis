using InsuranceRiskAnalysis.Core.Interfaces;

namespace InsuranceRiskAnalysis.WebUI.Services
{
    // Bu servis, WebUI'ın her zaman ana firma verilerini görmesini sağlar.
    public class FixedTenantService : ITenantService
    {
        // DbInitializer'da kullandığımız ID'nin aynısı!
        private const string AdminTenantId = "tenant-global-sigorta-01";

        public string GetTenantId()
        {
            return AdminTenantId;
        }

        public void SetTenantId(string tenantId)
        {
            // WebUI'da ID değiştirmeye gerek yok, o yüzden burası boş.
        }
    }
}
