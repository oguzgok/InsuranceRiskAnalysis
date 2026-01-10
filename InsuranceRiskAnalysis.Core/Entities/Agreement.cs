using InsuranceRiskAnalysis.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceRiskAnalysis.Core.Entities
{
    public class Agreement : BaseEntity
    {
        public string Name { get; private set; }
        public decimal RiskThreshold { get; private set; }
        public AgreementStatus Status { get; private set; }

        public Agreement(string tenantId, string name, decimal riskThreshold)
            : base(tenantId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Agreement name cannot be empty.");

            if (riskThreshold < 0)
                throw new DomainException("RiskThreshold cannot be negative.");

            Name = name;
            RiskThreshold = riskThreshold;
            Status = AgreementStatus.Active;
        }

        public void UpdateRiskThreshold(decimal newThreshold)
        {
            if (newThreshold < 0)
                throw new DomainException("RiskThreshold cannot be negative.");

            RiskThreshold = newThreshold;
        }

        public void Close()
        {
            Status = AgreementStatus.Closed;
        }
    }
}
