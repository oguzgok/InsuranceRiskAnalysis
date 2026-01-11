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
        public AppDbContext(DbContextOptions<AppDbContext> options, ITenantService tenantService) : base(options)
        {
            _currentTenantId = tenantService?.GetTenantId();
        }

        public DbSet<Partner> Partners { get; set; }
        public DbSet<Agreement> Agreements { get; set; }
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<RiskRule> RiskRules { get; set; } // Abstract sınıfı set ediyoruz

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Reflection ile metodumuzu buluyoruz
            var setQueryFilterMethod = GetType()
                .GetMethod(nameof(SetGlobalQueryFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // 2. Tüm entity'leri geziyoruz
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Eğer entity ITenantEntity interface'ini uyguluyorsa (Yani TenantId'si varsa)
                if (typeof(ITenantEntity).IsAssignableFrom(entityType.ClrType))
                {
                    // Metodu o tip için özelleştir (Örn: SetGlobalQueryFilter<Partner>)
                    var genericMethod = setQueryFilterMethod.MakeGenericMethod(entityType.ClrType);

                    // Metodu çalıştır
                    genericMethod.Invoke(this, new object[] { modelBuilder });
                }
            }

            // Polymorphism Ayarları
            modelBuilder.Entity<RiskRule>()
                .HasDiscriminator<string>("RuleType")
                .HasValue<KeywordRiskRule>("Keyword")
                .HasValue<AmountRiskRule>("Amount");

            // İlişki Ayarları
            modelBuilder.Entity<Partner>()
                .HasMany(p => p.Agreements)
                .WithOne(a => a.Partner)
                .HasForeignKey(a => a.PartnerId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

        // Bu metot Reflection ile dinamik olarak çağrılacak
        private void SetGlobalQueryFilter<T>(ModelBuilder builder) where T : class, ITenantEntity
        {
            // HasQueryFilter, EF Core'un standart metodudur
            builder.Entity<T>().HasQueryFilter(e => e.TenantId == _currentTenantId);
        }
    }
}
