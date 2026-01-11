using InsuranceRiskAnalysis.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceRiskAnalysis.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(AppDbContext context)
        {
            // Veritabanı yoksa oluştur (Migration yapıldıysa buna gerek yok ama garanti olsun)
            await context.Database.MigrateAsync();

            // Eğer veritabanında Partner varsa seed yapma (zaten veri var demektir)
            // Not: Global Query Filter devrede olduğu için IgnoreQueryFilters() kullanıyoruz.
            if (await context.Partners.IgnoreQueryFilters().AnyAsync())
            {
                return;
            }

            // 1. İş Ortağı (Partner) Oluştur
            var tenantId = "tenant-global-sigorta-01";
            var partner = new Partner
            {
                Name = "Global Sigorta A.Ş.",
                TaxNumber = "1234567890",
                ApiKey = "global-secret-key-123", // API isteğinde bunu kullanacağız!
                TenantId = tenantId
            };

            await context.Partners.AddAsync(partner);
            await context.SaveChangesAsync(); // ID oluşması için kaydet

            // 2. Anlaşma (Agreement) Oluştur
            var agreement = new Agreement
            {
                Title = "2024 Kasko Risk Anlaşması",
                Description = "Binek araçlar için standart kasko risk analizi",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1),
                BaseRiskAmount = 100, // Her sorguda en az 100 puan risk var
                PartnerId = partner.Id,
                TenantId = tenantId
            };

            await context.Agreements.AddAsync(agreement);
            await context.SaveChangesAsync();

            // 3. Risk Kuralları (Polymorphism Örneği Veriler)

            // Kural A: İçinde "Hasar" veya "Kaza" geçerse 500 puan ekle
            var keywordRule = new KeywordRiskRule
            {
                Name = "Kaza Kelime Kontrolü",
                Keyword = "kaza",
                RiskFactor = 500,
                AgreementId = agreement.Id,
                TenantId = tenantId
            };

            // Kural B: Tutar 20.000 TL'yi geçerse, aşan kısmın 2 katı kadar risk ekle
            var amountRule = new AmountRiskRule
            {
                Name = "Yüksek Tutar Limiti",
                ThresholdAmount = 20000,
                RiskMultiplier = 2,
                AgreementId = agreement.Id,
                TenantId = tenantId
            };

            await context.RiskRules.AddRangeAsync(keywordRule, amountRule);

            // Değişiklikleri kaydet
            await context.SaveChangesAsync();
        }
    }
}
