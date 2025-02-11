// src/app/dashboard/page.tsx
'use client';

import MainLayout from '@/components/layout/MainLayout';

export default function DashboardPage() {
  return (
    <MainLayout>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {/* Toplam Çalışan Kartı */}
        <div className="bg-white p-6 rounded-lg shadow">
          <h3 className="text-lg font-semibold text-gray-900">Toplam Çalışan</h3>
          <p className="mt-2 text-3xl font-bold text-blue-600">150</p>
        </div>

        {/* Departman Sayısı Kartı */}
        <div className="bg-white p-6 rounded-lg shadow">
          <h3 className="text-lg font-semibold text-gray-900">Departman Sayısı</h3>
          <p className="mt-2 text-3xl font-bold text-green-600">8</p>
        </div>

        {/* Oryantasyon Durumu Kartı */}
        <div className="bg-white p-6 rounded-lg shadow">
          <h3 className="text-lg font-semibold text-gray-900">Oryantasyon Tamamlama</h3>
          <p className="mt-2 text-3xl font-bold text-purple-600">75%</p>
        </div>
      </div>

      {/* Grafik veya Tablo eklenebilir */}
      <div className="mt-8 bg-white p-6 rounded-lg shadow">
        <h3 className="text-lg font-semibold text-gray-900 mb-4">
          Son Aktiviteler
        </h3>
        {/* Aktivite listesi veya grafik buraya gelecek */}
      </div>
    </MainLayout>
  );
}