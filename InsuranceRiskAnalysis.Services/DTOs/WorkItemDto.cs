namespace InsuranceRiskAnalysis.Services.DTOs
{
    public class WorkItemDto
    {
        public string Topic { get; set; }
        public string Content { get; set; }
        public decimal DeclaredAmount { get; set; }
        public int AgreementId { get; set; }
        // TenantId ve PartnerId API Key'den veya Token'dan çözülecek, buraya koymuyoruz (Güvenlik).
    }
}
