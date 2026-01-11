using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceRiskAnalysis.Core.Entities
{
    public class AmountRiskRule : RiskRule
    {
        public decimal ThresholdAmount { get; set; } // Örn: 10.000 TL üzeri
        public decimal RiskMultiplier { get; set; } // Örn: 1.5 kat risk

        public override decimal CalculateRisk(string content, decimal amount)
        {
            if (amount > ThresholdAmount)
            {
                return (amount * RiskMultiplier) - amount; // Eklenen risk
            }
            return 0;
        }
    }
}
