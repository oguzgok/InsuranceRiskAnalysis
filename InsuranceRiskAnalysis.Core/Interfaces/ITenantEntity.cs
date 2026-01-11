using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceRiskAnalysis.Core.Interfaces
{
    public interface ITenantEntity
    {
        string TenantId { get; set; } // İş ortağı (Kiracı) kimliği
    }
}
