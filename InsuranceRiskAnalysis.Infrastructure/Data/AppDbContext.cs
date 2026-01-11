using InsuranceRiskAnalysis.Core.Entities;
using InsuranceRiskAnalysis.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InsuranceRiskAnalysis.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        // TenantId'yi dışarıdan (Service veya Controller'dan) alacağız.
        // Bu sayede her sorguda "Where TenantId=..." yazmak zorunda kalmayacağız.
        private readonly string _currentTenantId;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // Normalde bu değeri bir "CurrentTenantService"den inject ederiz.
            // Şimdilik varsayılan bir değer veya null olabilir, WebApi katmanında bunu dolduracağız.
            _currentTenantId = "default-tenant";
        }

        // Constructor overloading (Dependency Injection için gerekli olabilir)
        //public AppDbContext(DbContextOptions<AppDbContext> options, ITenantService tenantService) : base(options)
        //{
        //    _currentTenantId = tenantService?.GetTenantId();
        //}

        public DbSet<Partner> Partners { get; set; }
        public DbSet<Agreement> Agreements { get; set; }
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<RiskRule> RiskRules { get; set; } // Abstract sınıfı set ediyoruz

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ITenantEntity interface'ini implemente eden TÜM entity'lere otomatik filtre koyuyoruz.
            // Böylece bir firma yanlışlıkla başka firmanın verisini asla göremez.
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ITenantEntity).IsAssignableFrom(entityType.ClrType))
                {
                    //modelBuilder.Entity(entityType.ClrType)
                    //    .AddQueryFilter<ITenantEntity>(e => e.TenantId == _currentTenantId);
                }
            }

            // RiskRule tablosunda "Discriminator" kolonu oluşturup hangi tip kural olduğunu orada tutacak.
            modelBuilder.Entity<RiskRule>()
                .HasDiscriminator<string>("RuleType")
                .HasValue<KeywordRiskRule>("Keyword")
                .HasValue<AmountRiskRule>("Amount");

            // İlişki Ayarları (Fluent API)
            modelBuilder.Entity<Partner>()
                .HasMany(p => p.Agreements)
                .WithOne(a => a.Partner)
                .HasForeignKey(a => a.PartnerId)
                .OnDelete(DeleteBehavior.Restrict); // Veri kaybını önlemek için Cascade silmeyi kapatıyoruz.

            base.OnModelCreating(modelBuilder);
        }
    }
}
