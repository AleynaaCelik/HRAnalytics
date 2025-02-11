// src/app/dashboard/page.tsx
'use client';

import MainLayout from '@/components/layout/MainLayout';
import { useEffect, useState } from 'react';
import api from '@/lib/axios';

type DashboardStats = {
  totalEmployees: number;
  totalDepartments: number;
  completionRate: number;
};

export default function DashboardPage() {
  const [stats, setStats] = useState<DashboardStats>({
    totalEmployees: 0,
    totalDepartments: 0,
    completionRate: 0
  });

  useEffect(() => {
    const fetchDashboardStats = async () => {
      try {
        const response = await api.get('/dashboard/stats');
        setStats(response.data.data);
      } catch (error) {
        console.error('Error fetching dashboard stats:', error);
      }
    };

    fetchDashboardStats();
  }, []);

  return (
    <MainLayout>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {/* Toplam Çalışan Kartı */}
        <div className="bg-white p-6 rounded-lg shadow hover:shadow-lg transition-shadow">
          <h3 className="text-lg font-semibold text-gray-900">Toplam Çalışan</h3>
          <p className="mt-2 text-3xl font-bold text-blue-600">
            {stats.totalEmployees}
          </p>
        </div>

        {/* Departman Sayısı Kartı */}
        <div className="bg-white p-6 rounded-lg shadow hover:shadow-lg transition-shadow">
          <h3 className="text-lg font-semibold text-gray-900">Departman Sayısı</h3>
          <p className="mt-2 text-3xl font-bold text-green-600">
            {stats.totalDepartments}
          </p>
        </div>

        {/* Oryantasyon Tamamlama Kartı */}
        <div className="bg-white p-6 rounded-lg shadow hover:shadow-lg transition-shadow">
          <h3 className="text-lg font-semibold text-gray-900">
            Oryantasyon Tamamlama
          </h3>
          <p className="mt-2 text-3xl font-bold text-purple-600">
            {stats.completionRate}%
          </p>
        </div>
      </div>

      {/* Yükleniyor durumu ve hata durumu için bildirimler ekleyebiliriz */}
    </MainLayout>
  );
}