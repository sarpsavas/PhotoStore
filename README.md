# 📸 PhotoStore

PhotoStore, fotoğrafçılık ekipmanları için geliştirilmiş modern bir ikinci el ilan platformudur.

Kullanıcılar kamera, lens, film, filtre ve çeşitli fotoğrafçılık aksesuarlarını listeleyebilir, inceleyebilir ve yönetebilir.

Bu proje özellikle backend tarafında modern .NET mimarileri ve yazılım geliştirme prensipleri uygulanarak geliştirilmiştir.

---

# 🚀 Project Purpose

PhotoStore'un amacı, fotoğrafçılıkla ilgilenen kullanıcılar için güvenilir ve düzenli bir ikinci el pazarı oluşturmaktır.

Platform üzerinden kullanıcılar:

- İlan oluşturabilir
- Ürün detaylarını inceleyebilir
- Fotoğraf yükleyebilir
- İlanları filtreleyebilir
- JWT tabanlı authentication sistemi ile güvenli şekilde işlem yapabilir

---

# 🛠 Technologies

## Backend

- .NET
- ASP.NET Core Web API
- Entity Framework Core
- REST API
- CQRS Pattern
- MediatR
- Clean Architecture
- Domain Driven Design (DDD)
- Unit of Work Pattern
- JWT Authentication

## Frontend

- HTML
- CSS
- JavaScript

## Database

- Microsoft SQL Server

---

# 🧠 Architecture

PhotoStore backend sistemi tamamen sıfırdan geliştirilmiştir.

Projede kullanılan temel mimari yaklaşımlar:

- Clean Architecture
- Domain Driven Design (DDD)
- CQRS
- Repository Pattern
- Unit of Work
- Dependency Injection

Katmanlı yapı sayesinde sistem:

- sürdürülebilir
- genişletilebilir
- test edilebilir

bir hale getirilmiştir.

---

# 🔐 Authentication

Sistemde JWT tabanlı authentication sistemi kullanılmaktadır.

Özellikler:

- Register/Login sistemi
- Token üretimi
- Yetkilendirme kontrolü
- Güvenli API erişimi

---

# ✨ Features

- User Register & Login
- JWT Authentication
- Listing Creation
- Listing Management
- Image Upload
- Product Categories
- Search & Filtering
- Responsive Frontend
- RESTful API
- CQRS Based Request Handling
- Secure API Structure

---

# 🧩 Listing Categories

```csharp
public enum ListingCategories
{
    Camera = 1,
    DigitalCamera = 11,
    AnalogCamera = 12,

    Objective = 2,

    Film = 3,
    ThirtyFivemm = 31,
    HundredTwentymm = 32,

    Accessory = 4,
    Tripods = 41,
    CameraBags = 42,
    MemoryCards = 43,
    BattariesChargers = 44,
    FlashLighting = 45,
    GimbalsStabilizers = 46,

    Filters = 5
}
```

---

# 📷 Project Images

## Home Page

![HomePage](images/homepage.png)

## Listing Detail

![ListingDetail](images/listing-detail.png)

## User Panel

![UserPanel](images/user-panel.png)

---

# ⚙️ Installation

## Clone Repository

```bash
git clone https://github.com/yourusername/PhotoStore.git
```

## Backend Setup

```bash
cd PhotoStore/API
```

### Apply Migration

```bash
dotnet ef database update
```

### Run Backend

```bash
dotnet run
```

---

## Frontend Setup

Frontend dosyalarını herhangi bir web server ile çalıştırabilirsiniz.

Örnek:

```bash
Live Server
```

veya

```bash
Open index.html
```

---

# 📌 Backend Highlights

Bu projede backend tarafı tamamen manuel olarak geliştirilmiştir.

Öne çıkan noktalar:

- Scalable Architecture
- Clean Code Structure
- CQRS Implementation
- MediatR Integration
- Unit of Work Pattern
- JWT Security
- RESTful API Design
- Layered Architecture
- Entity Framework Core Integration

---

# 📄 License

This project is licensed under the GNU General Public License v3.0 (GPL-3.0).

You are free to use, modify and distribute this project,  
but any modified/open distribution must also remain open-source.

---

# 👤 Developer

Developed by **YOUR_NAME**

GitHub: https://github.com/YOUR_GITHUB_USERNAME

LinkedIn: YOUR_LINKEDIN_URL

Email: YOUR_EMAIL

---

# ⭐ Notes

Frontend tarafı AI destekli geliştirilmiştir.

Backend mimarisi ve tüm backend geliştirme süreci tamamen tarafımdan yazılmıştır.
