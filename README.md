# Sigorta Risk Analizi ve Yönetim Sistemi (Insurance Risk Analysis)

Bu proje, sigorta sektöründeki birden fazla iş ortağından (Multi-Tenant) gelen talepleri toplayan, **polimorfik kurallarla dinamik risk analizi** yapan ve sonuçları **gerçek zamanlı (Real-time)** olarak yönetim paneline raporlayan dağıtık bir sistem simülasyonudur.

## Proje Hakkında & Mimari Kararlar

Proje **.NET 8** üzerinde, **Onion (Clean) Architecture** prensipleriyle geliştirilmiştir.

### Öne Çıkan Özellikler

1.  **Polymorphism & Strategy Pattern:** Risk motoru `if-else` blokları yerine OOP kalıtımı kullanır. Yeni bir kural eklendiğinde (örn: *Lokasyon Riski*) ana kod değişmez (Open/Closed Principle).
2.  **Gerçek Multi-Tenancy:** Veri güvenliği **EF Core Global Query Filters** ile sağlanmıştır.
    * **Firma A**'nın verisi veritabanında **Firma B**'den tamamen izoledir.
    * Yazılım seviyesinde değil, Veritabanı (ORM) seviyesinde izolasyon vardır.
3.  **Real-Time İletişim:** Risk analizi tamamlandığında, Dashboard **SignalR** ile anlık güncellenir.
4.  **No-Magic Strings:** `TenantId` ve `ConnectionStrings` kod içine gömülmemiştir (Hardcoded değildir), Dependency Injection ile dinamik yönetilir.
5.  **Otomatik Seed Data:** Uygulama ayağa kalkarken veritabanını oluşturur ve **2 Farklı Firma** için test verilerini otomatik yükler.

---

## Kurulum ve Çalıştırma

### 1. Veritabanı Ayarı
Yerel SQL Server (veya LocalDB) kullanılır. Yoksa Docker ile SQL'li ayağa kaldırılıp kullanabilirsiniz.
`WebApi/Program.cs` ve `WebUI/Program.cs` içindeki bağlantı cümlesini kontrol edin:
```csharp
"Server=localhost;Database=InsuranceRiskDb;Trusted_Connection=True;TrustServerCertificate=True;"```

### 2. Başlatma

İki ayrı terminal açın ve aşağıdaki komutları çalıştırın:

Terminal 1 (Backend - API):
Bash

cd WebApi
dotnet run
Port: http://localhost:5291/swagger/index.html (Swagger: /swagger)

Terminal 2 (Frontend - Dashboard):
Bash

cd WebUI
dotnet run
Port: http://localhost:5xxx (Konsolda yazar)

### Test Senaryoları

Sistemin hem Risk Motorunun hem de Veri İzolasyonunun çalıştığını kanıtlamak için aşağıdaki iki senaryoyu uygulayın.
# Senaryo 1: Ana Firma (Global Sigorta) Testi

Dashboard (WebUI), varsayılan olarak bu firmanın yönetim panelidir.

    API Key: global-secret-key-123

    Beklenen: İstek atıldığında Dashboard anlık olarak güncellenmelidir.

cURL Komutu:
```
curl -X 'POST' \
  'http://localhost:5000/api/WorkItems' \
  -H 'accept: */*' \
  -H 'X-ApiKey: global-secret-key-123' \
  -H 'Content-Type: application/json' \
  -d '{
  "topic": "Lüks Araç Kaskosu",
  "content": "Müşterinin kaza geçmişi kabarık.",
  "declaredAmount": 25000,
  "agreementId": 1
}'
```
Sonuç: Tabloya "Lüks Araç Kaskosu" düşer, Risk Skoru hesaplanır.
# Senaryo 2: Rakip Firma (İzolasyon Testi)

Sisteme ikinci bir firma olarak istek atacağız. Bu firma veritabanında var ama Dashboard'da yetkisi yok.

    API Key: rakip-secret-key-999

    Beklenen: API 200 OK döner ve veriyi kaydeder. ANCAK Dashboard'da hiçbir değişiklik olmamalıdır. Bu, Firma A'nın Firma B'nin verisini görmüyor (Veri İzolasyonu).

cURL Komutu:
```
curl -X 'POST' \
  'http://localhost:5000/api/WorkItems' \
  -H 'accept: */*' \
  -H 'X-ApiKey: rakip-secret-key-999' \
  -H 'Content-Type: application/json' \
  -d '{
  "topic": "Rakip Firma Depo Sigortası",
  "content": "Depoda yangın riski var.",
  "declaredAmount": 5000,
  "agreementId": 2
}'
```
Sonuç: Veri veritabanına "tenant-rakip-sigorta-02" ID'si ile yazılır ama "tenant-global-sigorta-01"e ayarlı Dashboard'da görünmez.
# Mimari Yapı

📦 InsuranceRiskAnalysis
 ┣ 📂 Core          -> Domain Entities (Saf C#)
 ┣ 📂 Infrastructure-> EF Core, Migrations, Seed Data
 ┣ 📂 Services      -> Business Logic
 ┣ 📂 WebApi        -> Middleware (Auth), Controllers
 ┗ 📂 WebUI         -> MVC Dashboard, SignalR Client

# Geliştirici Notu: Bu proje, Clean Architecture ve SOLID prensiplerine tam uyumluluk gözetilerek hazırlanmıştır.