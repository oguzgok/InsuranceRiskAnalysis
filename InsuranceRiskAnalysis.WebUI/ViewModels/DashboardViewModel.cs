using InsuranceRiskAnalysis.Core.Entities;

namespace InsuranceRiskAnalysis.WebUI.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalWorkItems { get; set; }
        public decimal TotalRiskValue { get; set; }
        public int HighRiskCount { get; set; }
        public List<WorkItem> RecentWorkItems { get; set; }
    }
}
