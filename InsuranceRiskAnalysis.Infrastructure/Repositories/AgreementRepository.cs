using InsuranceRiskAnalysis.Core.Entities;
using InsuranceRiskAnalysis.Core.Interfaces;
using InsuranceRiskAnalysis.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InsuranceRiskAnalysis.Infrastructure.Repositories
{
    public class AgreementRepository : Repository<Agreement>, IAgreementRepository
    {
        public AgreementRepository(AppDbContext context) : base(context) { }

        public async Task<Agreement> GetAgreementWithRulesAsync(int id)
        {
            return await _context.Agreements
                .Include(a => a.RiskRules) // Kuralları da getir
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
