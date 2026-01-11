using InsuranceRiskAnalysis.Core.Entities;
using InsuranceRiskAnalysis.Core.Enums;
using InsuranceRiskAnalysis.Core.Interfaces;
using InsuranceRiskAnalysis.WebUI.Models;
using InsuranceRiskAnalysis.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InsuranceRiskAnalysis.WebUI.Controllers;

public class DashboardController : Controller
{
    private readonly IRepository<WorkItem> _workItemRepo;

    public DashboardController(IRepository<WorkItem> workItemRepo)
    {
        _workItemRepo = workItemRepo;
    }

    // Parametreler ekledik: search (arama), statusFilter (durum filtresi)
    public async Task<IActionResult> Index(string search, RiskStatus? statusFilter)
    {
        var allItems = await _workItemRepo.GetAllAsync();

        // LINQ ile hafýzada filtreleme (Veri çoksa bu iþlem Service/Db tarafýnda yapýlmalý)
        var query = allItems.AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(x => x.Topic.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                     x.Content.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        if (statusFilter.HasValue)
        {
            query = query.Where(x => x.Status == statusFilter.Value);
        }

        var filteredList = query.OrderByDescending(x => x.CreatedDate).ToList();

        var model = new DashboardViewModel
        {
            TotalWorkItems = allItems.Count(), // Toplam sayý deðiþmesin
            TotalRiskValue = filteredList.Sum(x => x.CalculatedRiskScore),
            HighRiskCount = filteredList.Count(x => x.Status == RiskStatus.HighRisk || x.Status == RiskStatus.Rejected),
            RecentWorkItems = filteredList.Take(10).ToList() // Filtrelenmiþ liste
        };

        // View'da filtreleri korumak için
        ViewBag.Search = search;
        ViewBag.StatusFilter = statusFilter;

        return View(model);
    }
}
