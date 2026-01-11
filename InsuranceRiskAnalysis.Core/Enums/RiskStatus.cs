using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceRiskAnalysis.Core.Enums
{
    public enum RiskStatus
    {
        Pending = 0,
        LowRisk = 1,
        MediumRisk = 2,
        HighRisk = 3,
        Rejected = 4
    }
}
