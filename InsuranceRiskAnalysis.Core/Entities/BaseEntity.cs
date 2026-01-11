using InsuranceRiskAnalysis.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceRiskAnalysis.Core.Entities
{
    public abstract class BaseEntity : ITenantEntity
    {
        public int Id { get; set; }
        public string TenantId { get; set; } // Hangi firmanın verisi?
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false; // Soft Delete (Veri bütünlüğü için silmiyoruz)
    }
}
