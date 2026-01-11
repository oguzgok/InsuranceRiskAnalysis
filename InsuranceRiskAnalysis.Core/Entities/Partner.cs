
namespace InsuranceRiskAnalysis.Core.Entities
{
    public class Partner : BaseEntity
    {
        public string Name { get; set; }
        public string TaxNumber { get; set; }
        public string ApiKey { get; set; } // Web servis isteklerinde kimlik doğrulama için

        // Navigation Properties
        public virtual ICollection<Agreement> Agreements { get; set; }
    }
}
