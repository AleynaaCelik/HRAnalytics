'use client';

import { useEffect, useState } from 'react';
import api from '@/lib/axios';

export default function Home() {
  return (
    <main className="min-h-screen p-8">
      <div className="max-w-7xl mx-auto">
        <h1 className="text-3xl font-bold mb-8">HR Analytics Dashboard</h1>
        
        {/* Ana İçerik */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {/* Çalışan Kartı */}
          <div className="bg-white p-6 rounded-lg shadow-md">
            <h2 className="text-xl font-semibold mb-4">Çalışanlar</h2>
            <div className="text-4xl font-bold">0</div>
            <p className="text-gray-600 mt-2">Toplam Çalışan</p>
          </div>

          {/* Departman Kartı */}
          <div className="bg-white p-6 rounded-lg shadow-md">
            <h2 className="text-xl font-semibold mb-4">Departmanlar</h2>
            <div className="text-4xl font-bold">0</div>
            <p className="text-gray-600 mt-2">Toplam Departman</p>
          </div>

          {/* Oryantasyon Kartı */}
          <div className="bg-white p-6 rounded-lg shadow-md">
            <h2 className="text-xl font-semibold mb-4">Oryantasyon</h2>
            <div className="text-4xl font-bold">%0</div>
            <p className="text-gray-600 mt-2">Ortalama Tamamlanma</p>
          </div>
        </div>
      </div>
    </main>
  );
}