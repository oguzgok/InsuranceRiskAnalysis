using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceRiskAnalysis.Core.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        // multi tenant yapısı için tenantId base e eklendi
        public string TenantId { get; protected set; } = string.Empty;

        protected BaseEntity() { }

        protected BaseEntity(string tenantId)
        {
            TenantId = tenantId;
        }
    }
}
