using Microsoft.AspNetCore.SignalR;

namespace InsuranceRiskAnalysis.WebApi.Hubs
{
    // Dashboard (WebUI) buraya bağlanacak ve dinleyecek.
    public class RiskHub : Hub
    {
        // Client'ların tetikleyebileceği metotlar buraya yazılır.
        // Biz server'dan client'a push yapacağız, o yüzden şimdilik boş kalabilir.
        // Ama loglama için override edelim.
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
    }
}
