using InsuranceRiskAnalysis.Core.Entities;
using InsuranceRiskAnalysis.Core.Interfaces;

namespace InsuranceRiskAnalysis.Services.AgreementServices
{
    public class AgreementService : IAgreementService
    {
        private readonly IAgreementRepository _agreementRepo;
        private readonly IRepository<RiskRule> _riskRuleRepo;

        public AgreementService(IAgreementRepository agreementRepo, IRepository<RiskRule> riskRuleRepo)
        {
            _agreementRepo = agreementRepo;
            _riskRuleRepo = riskRuleRepo;
        }

        public async Task AddRiskRuleAsync(int agreementId, RiskRule rule)
        {
            var agreement = await _agreementRepo.GetByIdAsync(agreementId);
            if (agreement == null) throw new Exception("Anlaşma bulunamadı.");

            rule.AgreementId = agreementId;
            await _riskRuleRepo.AddAsync(rule);
        }

        public async Task<Agreement> CreateAgreementAsync(Agreement agreement)
        {
            // Validasyonlar buraya eklenebilir (FluentValidation önerilir ama manuel yapalım)
            if (string.IsNullOrEmpty(agreement.Title)) throw new Exception("Başlık zorunlu.");

            await _agreementRepo.AddAsync(agreement);
            return agreement;
        }

        public async Task<Agreement> GetAgreementWithRulesAsync(int id)
        {
            return await _agreementRepo.GetAgreementWithRulesAsync(id);
        }
    }
}
