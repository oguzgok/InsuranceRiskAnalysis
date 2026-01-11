namespace InsuranceRiskAnalysis.Core.Entities
{
    public class KeywordRiskRule : RiskRule
    {
        public string Keyword { get; set; } // Örn: "Kaza", "Hasar"
        public decimal RiskFactor { get; set; } // Örn: 100 birim risk ekle

        public override decimal CalculateRisk(string content, decimal amount)
        {
            if (!string.IsNullOrEmpty(content) && content.Contains(Keyword, StringComparison.OrdinalIgnoreCase))
            {
                return RiskFactor;
            }
            return 0;
        }
    }
}
