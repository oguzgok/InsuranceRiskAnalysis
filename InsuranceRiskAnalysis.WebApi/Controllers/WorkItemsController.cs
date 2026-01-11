using InsuranceRiskAnalysis.Core.Entities;
using InsuranceRiskAnalysis.Core.Interfaces;
using InsuranceRiskAnalysis.Services.DTOs;
using InsuranceRiskAnalysis.WebApi.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace InsuranceRiskAnalysis.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkItemsController : ControllerBase
    {
        private readonly IRiskService _riskService;
        private readonly IHubContext<RiskHub> _hubContext; // SignalR Context

        public WorkItemsController(IRiskService riskService, IHubContext<RiskHub> hubContext)
        {
            _riskService = riskService;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkItem([FromBody] WorkItemDto dto)
        {
            // Mapper kullanabilirsin (AutoMapper) ama manuel yapalım şimdilik
            var workItem = new WorkItem
            {
                Topic = dto.Topic,
                Content = dto.Content,
                DeclaredAmount = dto.DeclaredAmount,
                AgreementId = dto.AgreementId
                // TenantId, DbContext tarafından otomatik set edilecek (Middleware sayesinde)
            };

            var result = await _riskService.AnalyzeAndSaveAsync(workItem);

            // "ReceiveRiskUpdate" adında bir event fırlatıyoruz. Dashboard bunu dinleyecek.
            await _hubContext.Clients.All.SendAsync("ReceiveRiskUpdate", new
            {
                Topic = result.Topic,
                Score = result.CalculatedRiskScore,
                Status = result.Status.ToString(),
                Date = DateTime.Now
            });

            return Ok(result);
        }
    }
}
