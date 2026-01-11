namespace InsuranceRiskAnalysis.Core.Entities
{
    public class Agreement : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal BaseRiskAmount { get; set; } // Anlaşma bazlı taban risk

        public int PartnerId { get; set; }
        public virtual Partner Partner { get; set; }

        // Anlaşmaya bağlı risk kuralları (Polymorphism burada devreye girecek)
        public virtual ICollection<RiskRule> RiskRules { get; set; }
    }
}
