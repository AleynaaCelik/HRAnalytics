

# HR Analytics - İnsan Kaynakları Oryantasyon Takip Sistemi

## 📋 Proje Özeti  

Modern insan kaynakları yönetimini kolaylaştıran, çalışan oryantasyon süreçlerini etkin bir şekilde takip eden web tabanlı bir yönetim sistemi.

## 🎯 Temel Özellikler  

### 👥 Çalışan Yönetimi  
- Detaylı çalışan profil yönetimi  
- Oryantasyon süreç takibi  
- Gelişim ve performans izleme  
- Çalışan aktivite geçmişi  

### 🏢 Departman Yönetimi  
- Departman bazlı organizasyon yapısı  
- Departman-çalışan ilişkileri  
- Departman bazlı raporlama  

### 📈 Oryantasyon Süreç Yönetimi  
- İnteraktif ilerleme takibi  
- Modül bazlı performans değerlendirme  
- Gerçek zamanlı raporlama  
- Otomatik durum güncellemeleri  

## 💻 Kullanılan Teknolojiler  

### Backend  
- **.NET 8**
  - Entity Framework Core  
  - JWT Authentication  
  - Fluent Validation  
  - AutoMapper  
  - Serilog  

### Frontend  
- **Next.js 14**
  - TypeScript  
  - TailwindCSS  
  - Axios  
  - React Query  

### Veritabanı & DevOps  
- **PostgreSQL**  
- **Docker**  
- **Docker Compose**  

## 🚀 Kurulum Adımları  

### Ön Gereksinimler  
- .NET 8 SDK  
- Node.js  
- Docker Desktop  
- PostgreSQL (Docker ile)  

### Kurulum  
1. **Repo Klonlama**  
```bash
git clone https://github.com/yourusername/hr-analytics.git
cd hr-analytics
```

2. **Backend Kurulumu**  
```bash
cd HRAnalytics.API
dotnet restore
dotnet run
```

3. **Frontend Kurulumu**  
```bash
cd FRONTEND
npm install
npm run dev
```

4. **Docker & Veritabanı**  
```bash
docker-compose up -d
```

## 🔑 Ortam Değişkenleri  

`.env.local` dosyası (Frontend için):  
```env
NEXT_PUBLIC_API_URL=http://localhost:5000
```

## 📌 API Endpoint'leri  

| Metod | Endpoint | Açıklama |
|-------|----------|-----------|
| GET | `/api/v1/employees` | Çalışan listesi |
| GET | `/api/v1/departments` | Departman listesi |
| POST | `/api/v1/employees/{id}/progress` | İlerleme güncelleme |

## 🤝 Katkıda Bulunma  

1. Projeyi fork'layın  
2. Yeni bir branch oluşturun (`git checkout -b feature/amazing-feature`)  
3. Değişikliklerinizi commit'leyin (`git commit -m 'feat: Add amazing feature'`)  
4. Branch'inizi push'layın (`git push origin feature/amazing-feature`)  
5. Pull Request oluşturun  



## 👨‍💻 Geliştiriciler  
- [Aleyna ÇELİK ]((https://github.com/AleynaaCelik))  

## 📞 İletişim  
- Email: aleynaa.celik0@gmail.com 
- LinkedIn: [(https://www.linkedin.com/in/aleyna-%C3%A7elik/)]

---

### 🌟 Star Verin!  
Beğendiyseniz projeye star vermeyi unutmayın!




![1](https://github.com/user-attachments/assets/cd6dbf13-f657-40d5-81c7-800f75b793b5)


![1 1](https://github.com/user-attachments/assets/cc079149-93e9-4fc5-a285-baf7b62defac)

![frontendd](https://github.com/user-attachments/assets/877a3538-b04a-4b1a-a242-4c8f6d95e175)

