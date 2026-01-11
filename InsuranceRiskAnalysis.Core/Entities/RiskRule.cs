using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceRiskAnalysis.Core.Entities
{
    // Kalıtım stratejisi: Table-Per-Hierarchy (TPH) kullanacağız.
    public abstract class RiskRule : BaseEntity
    {
        public string Name { get; set; }
        public int AgreementId { get; set; }
        public virtual Agreement Agreement { get; set; }

        // Bu metot ezilecek (Polymorphism)
        public abstract decimal CalculateRisk(string content, decimal amount);
    }
}
