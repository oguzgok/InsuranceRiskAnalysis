using InsuranceRiskAnalysis.Core.Enums;

namespace InsuranceRiskAnalysis.Services.DTOs
{
    public class RiskAnalysisResultDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public decimal CalculatedRiskScore { get; set; }
        public string RiskStatus { get; set; } // Enum string karşılığı
        public DateTime AnalyzedAt { get; set; }
    }
}
