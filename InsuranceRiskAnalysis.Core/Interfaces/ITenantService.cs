namespace InsuranceRiskAnalysis.Core.Interfaces
{
    public interface ITenantService
    {
        string GetTenantId();
        void SetTenantId(string tenantId);
    }
}
