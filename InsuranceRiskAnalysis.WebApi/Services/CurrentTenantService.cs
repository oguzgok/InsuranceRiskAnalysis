using InsuranceRiskAnalysis.Core.Interfaces;

namespace InsuranceRiskAnalysis.WebApi.Services
{
    // Scoped olarak tanımlanacak: Her HTTP isteği için bir tane oluşur.
    public class CurrentTenantService : ITenantService
    {
        private string _tenantId;

        public string GetTenantId()
        {
            return _tenantId;
        }

        public void SetTenantId(string tenantId)
        {
            _tenantId = tenantId;
        }
    }
}
