
# .Net Enterprise API - Kurumsal .NET 10 Web API Şablonu

EnterpriseAPI, modern yazılım geliştirme prensipleriyle inşa edilmiş, ölçeklenebilir, güvenli ve test edilebilir bir .NET 10 Web API şablonudur. Katmanlı mimari (Clean Architecture), JWT kimlik doğrulama, PostgreSQL veritabanı, Docker desteği ve daha birçok kurumsal özelliği kutudan çıkar sunar.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15-4169E1?style=flat&logo=postgresql)
![Docker](https://img.shields.io/badge/Docker-✓-2496ED?style=flat&logo=docker)
![JWT](https://img.shields.io/badge/Auth-JWT-000000?style=flat&logo=json-web-tokens)
![License](https://img.shields.io/badge/License-MIT-green)

---

## 🚀 Özellikler

| Alan | Teknolojiler / Yaklaşımlar |
|------|-----------------------------|
| **Mimari** | Clean Architecture (Katmanlı Yapı) |
| **Framework** | .NET 10 |
| **Veritabanı** | PostgreSQL, Entity Framework Core, Npgsql |
| **Kimlik & Yetkilendirme** | ASP.NET Core Identity, JWT Bearer Authentication |
| **Validasyon** | FluentValidation |
| **Loglama** | Serilog (Console, File, Seq entegrasyonu) |
| **API Dokümantasyonu** | Swagger / OpenAPI |
| **Sağlık Kontrolü** | Health Checks (PostgreSQL bağlantı testi) |
| **CORS** | Yapılandırılabilir CORS politikaları |
| **Repository Deseni** | Generic Repository, Unit of Work (opsiyonel) |
| **Soft Delete** | Mantıksal silme (`IsDeleted` alanı) |
| **Audit** | Oluşturma/güncelleme zamanı ve kullanıcı takibi |
| **Hata Yönetimi** | Global Exception Handling Middleware |
| **Container** | Docker & Docker Compose desteği |
| **Test** | xUnit, Moq, FluentAssertions (örnek testler) |

---

## 📁 Proje Yapısı


EnterpriseAPI/
├── src/
│   ├── EnterpriseAPI.Api          # Presentation katmanı (Controllers, Middleware)
│   ├── EnterpriseAPI.Application   # İş mantığı, DTO'lar, Validator'lar, Servis arayüzleri
│   ├── EnterpriseAPI.Domain        # Entity'ler, Domain modelleri, Repository arayüzleri
│   ├── EnterpriseAPI.Infrastructure# Veritabanı, Repository implementasyonları, DbContext
│   └── EnterpriseAPI.Shared        # Yardımcı sınıflar, Extension'lar
├── tests/
│   └── EnterpriseAPI.Tests         # Birim ve entegrasyon testleri
├── docker-compose.yml              # Docker Compose yapılandırması
├── Dockerfile                       # API için Docker imaj tanımı
├── Directory.Build.props            # Merkezi derleme ayarları
├── Directory.Packages.props         # Merkezi paket sürüm yönetimi
└── README.md                        # Bu dosya



---

## ⚙️ Başlarken

### Gereksinimler

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10) (veya .NET 10 preview)
- [PostgreSQL](https://www.postgresql.org/download/) (15 veya üzeri)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (isteğe bağlı)
- [Git](https://git-scm.com/)

### Kurulum

1. **Depoyu klonlayın**
   ```bash
   git clone https://github.com/ugurkus/DotnetEnterpriseApi.git
   cd DotnetEnterpriseApi

2. **Veritabanını oluşturun**
PostgreSQL'de EnterpriseAPIDB adında bir veritabanı oluşturun veya connection string'i kendi ayarlarınıza göre düzenleyin.

3. **appsettings.json dosyasını yapılandırın**
	 ```bash
	 ConnectionStrings": {
	  "DefaultConnection": "Host=localhost;Port=5432;Database=EnterpriseAPIDB;Username=postgres;Password=yourpassword"
	},
	"JwtSettings": {
	  "Secret": "bu-cok-gizli-ve-en-az-32-karakterli-bir-anahtar",
	  "Issuer": "EnterpriseAPI",
	  "Audience": "EnterpriseAPIClients",
	  "ExpiryMinutes": 60
	}

4. **Migration'ları uygulayın**
	```bash
    cd src/EnterpriseAPI.Api
    dotnet ef database update

5. **Projeyi çalıştırın**
	```bash
	dotnet run
	
API artık https://localhost:5001 adresinde hazır. Swagger UI için https://localhost:5001/swagger adresini ziyaret edin.

## 🐳 Docker ile Çalıştırma

Proje, Docker ve Docker Compose ile kolayca ayağa kaldırılabilir.

1.  **Docker Compose ile başlatın**
	
		docker-compose up -d
	

	Bu komut:
    
    -   PostgreSQL container'ını başlatır (port `5432`).
        
    -   API container'ını build edip başlatır (port `5000`).
        
3.  **Logları izleyin**
    
    ```bash
    
    docker-compose logs -f api
    
4.  **Durdurmak için**
    
    ```bash
    
    docker-compose down

> **Not:** Docker kullanırken connection string'de `Host=postgres` yazdığından emin olun (servis adı). Bu ayar `docker-compose.yml` içinde environment variable olarak tanımlanmıştır.

----------

## 🔐 Kimlik Doğrulama ve Yetkilendirme

Proje JWT tabanlı kimlik doğrulama kullanır.

### Kullanıcı Kaydı

	POST /api/auth/register
	Content-Type: application/json
	{
	 "email": "test@example.com",
	 "password": "Test123!",
	 "firstName": "Test",
	 "lastName": "User"
	}

### Kullanıcı Girişi

	POST /api/auth/login
	Content-Type: application/json
	{
	 "email": "test@example.com",
	 "password": "Test123!"
	}

Başarılı giriş sonucu bir JWT token döner. Bu token'ı Swagger'da "Authorize" butonuna `Bearer {token}` şeklinde girerek korumalı endpoint'leri çağırabilirsiniz.

### Roller

-   **Admin** rolü özel yetkilere sahiptir (örneğin rol yönetimi). İlk çalıştırmada `admin@example.com` / `Admin123!` kullanıcısı otomatik oluşturulur.
    

----------

## 📚 API Dokümantasyonu

API, Swagger/OpenAPI ile tam dokümante edilmiştir. Çalışan uygulamada `/swagger` adresini ziyaret ederek tüm endpoint'leri görüntüleyebilir ve test edebilirsiniz.

Başlıca endpoint grupları:
| Controller | Açıklama |Yetki |
|--|--|--|
|`api/v1/products` |Ürün CRUD işlemleri |Admin (yazma işlemleri)
|`api/auth`|Kayıt, giriş, token alma|Herkese açık|
|`api/role`|Rol yönetimi (CRUD)|Admin|
|`api/userrole`|Kullanıcılara rol atama|Admin|
|`health`|Uygulama sağlık kontrolü|Herkese açık
----------

## 🧪 Testler

Test projesi `tests/EnterpriseAPI.Tests` klasöründe bulunur. Birim testleri çalıştırmak için:

bash

dotnet test

Testlerde şunlar örneklenmiştir:

-   `ProductService` birim testleri (Moq ile)
    
-   FluentValidation kuralları
    
-   Repository mock'ları
    

----------

## 🤝 Katkıda Bulunma

Katkılarınızı memnuniyetle karşılıyoruz! Lütfen şu adımları izleyin:

1.  Bu depoyu fork edin.
    
2.  Yeni bir dal oluşturun (`git checkout -b feature/yeniOzellik`).
    
3.  Değişikliklerinizi commit edin (`git commit -m 'Yeni özellik eklendi'`).
    
4.  Dalınıza push yapın (`git push origin feature/yeniOzellik`).
    
5.  Bir Pull Request açın.
    

----------

## 📄 Lisans

Bu proje MIT lisansı ile lisanslanmıştır. Detaylar için `LICENSE` dosyasına bakın.

----------

## 📧 İletişim

Sorularınız veya önerileriniz için [e-posta adresiniz] veya [GitHub Issues](https://github.com/kullanici/EnterpriseAPI/issues) üzerinden bize ulaşabilirsiniz.

----------

⭐️ Bu projeyi beğendiyseniz GitHub'da yıldızlamayı unutmayın!