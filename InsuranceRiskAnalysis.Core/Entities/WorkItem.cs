using InsuranceRiskAnalysis.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceRiskAnalysis.Core.Entities
{
    public class WorkItem : BaseEntity
    {
        public string Topic { get; set; } // İş konusu başlığı
        public string Content { get; set; } // Detay (Risk kelimeleri burada aranacak)
        public decimal DeclaredAmount { get; set; } // Bildirilen tutar

        public int AgreementId { get; set; }
        public virtual Agreement Agreement { get; set; }

        // Analiz Sonuçları
        public decimal CalculatedRiskScore { get; set; }
        public RiskStatus Status { get; set; } // Onay, Red, İnceleme
    }
}
