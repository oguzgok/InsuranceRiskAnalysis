using InsuranceRiskAnalysis.Core.Entities;

namespace InsuranceRiskAnalysis.Core.Interfaces
{
    // DTO referansları için Core katmanına DTO taşımak yerine,
    // burada Entity alıp verme veya generic tipler kullanılabilir. 
    // Ancak basitlik adına burada metot imzasını genel tutuyoruz.
    // Biz burada parametre olarak Entity veya primitive type kullanacağız.

    public interface IRiskService
    {
        Task<WorkItem> AnalyzeAndSaveAsync(WorkItem workItem);
        Task<decimal> CalculateTotalRiskAsync(int agreementId, string content, decimal amount);
    }
}
