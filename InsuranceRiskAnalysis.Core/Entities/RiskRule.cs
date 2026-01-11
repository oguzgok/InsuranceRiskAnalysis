namespace InsuranceRiskAnalysis.Core.Entities
{
    public abstract class RiskRule : BaseEntity
    {
        public string Name { get; set; }
        public int AgreementId { get; set; }
        public virtual Agreement Agreement { get; set; }

        // Bu metot ezilecek (Polymorphism)
        public abstract decimal CalculateRisk(string content, decimal amount);
    }
}
