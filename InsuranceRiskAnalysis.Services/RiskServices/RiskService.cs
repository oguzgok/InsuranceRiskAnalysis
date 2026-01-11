using InsuranceRiskAnalysis.Core.Entities;
using InsuranceRiskAnalysis.Core.Enums;
using InsuranceRiskAnalysis.Core.Interfaces;

namespace InsuranceRiskAnalysis.Services.RiskServices
{
    public class RiskService : IRiskService
    {
        private readonly IRepository<WorkItem> _workItemRepo;
        private readonly IAgreementRepository _agreementRepo;

        // Repository pattern kullanıyoruz.
        public RiskService(IRepository<WorkItem> workItemRepo, IAgreementRepository agreementRepo)
        {
            _workItemRepo = workItemRepo;
            _agreementRepo = agreementRepo;
        }

        public async Task<WorkItem> AnalyzeAndSaveAsync(WorkItem workItem)
        {
            // 1. Risk Hesapla
            var totalRisk = await CalculateTotalRiskAsync(workItem.AgreementId, workItem.Content, workItem.DeclaredAmount);

            // 2. Durumu Belirle (Basit bir iş kuralı)
            workItem.CalculatedRiskScore = totalRisk;
            workItem.Status = DetermineStatus(totalRisk);

            // 3. Kaydet
            await _workItemRepo.AddAsync(workItem);

            return workItem;
        }

        public async Task<decimal> CalculateTotalRiskAsync(int agreementId, string content, decimal amount)
        {
            // Anlaşmayı ve kuralları çekiyoruz (Eager Loading gerekli)
            // Generic Repository'de "Include" yeteneği eklemediysek, burada DbContext'e erişim 
            // veya Repository'e "GetWithIncludes" metodu eklemek gerekebilir.
            // Şimdilik FindAsync ile mantığı kuralım, Repository'i basit tuttuk.

            var agreement = await _agreementRepo.GetAgreementWithRulesAsync(agreementId);
            if (agreement == null) throw new Exception("Agreement not found!");
            // *Burada Include("RiskRules") yapılması lazım, altyapı kısmında generic repoya include eklemeyi not et.*

            decimal totalRisk = agreement.BaseRiskAmount;

            // Bu döngüde KeywordRule mu AmountRule mu olduğunu kontrol etmemize gerek yok!
            // .CalculateRisk() metodu zaten override edildi.
            foreach (var rule in agreement.RiskRules)
            {
                totalRisk += rule.CalculateRisk(content, amount);
            }

            return totalRisk;
        }

        private RiskStatus DetermineStatus(decimal score)
        {
            if (score <= 0) return RiskStatus.LowRisk;
            if (score < 1000) return RiskStatus.MediumRisk;
            if (score < 5000) return RiskStatus.HighRisk;
            return RiskStatus.Rejected;
        }
    }
}
