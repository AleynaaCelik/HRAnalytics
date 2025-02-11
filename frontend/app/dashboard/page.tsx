'use client';
import MainLayout from '@/components/layout/MainLayout'; // veya
// import MainLayout from '../../../components/layout/MainLayout';

export default function DashboardPage() {
  return (
    <MainLayout>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {/* Kartlar */}
        ...
      </div>
    </MainLayout>
  );
}