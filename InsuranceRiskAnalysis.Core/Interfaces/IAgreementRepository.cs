using InsuranceRiskAnalysis.Core.Entities;

namespace InsuranceRiskAnalysis.Core.Interfaces
{
    // Generic repository'i miras alıp genişletiyoruz
    public interface IAgreementRepository : IRepository<Agreement>
    {
        Task<Agreement> GetAgreementWithRulesAsync(int id);
    }
}
