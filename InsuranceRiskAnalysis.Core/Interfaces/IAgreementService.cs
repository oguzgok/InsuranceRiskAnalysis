using InsuranceRiskAnalysis.Core.Entities;

namespace InsuranceRiskAnalysis.Core.Interfaces
{
    public interface IAgreementService
    {
        Task<Agreement> CreateAgreementAsync(Agreement agreement);
        Task AddRiskRuleAsync(int agreementId, RiskRule rule);
        Task<Agreement> GetAgreementWithRulesAsync(int id);
    }
}
