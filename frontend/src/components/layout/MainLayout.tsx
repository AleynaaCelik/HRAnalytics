// src/components/layout/MainLayout.tsx
'use client';

import { useAuthStore } from '@/store/auth';
import { useRouter } from 'next/navigation';
import { ReactNode, useEffect } from 'react';

interface MainLayoutProps {
  children: ReactNode;
}
export default function MainLayout({ children }: MainLayoutProps) {
  const isAuthenticated = useAuthStore((state: { isAuthenticated: boolean }) => state.isAuthenticated);
  const router = useRouter();

  useEffect(() => {
    if (!isAuthenticated) {
      router.push('/login');
    }
  }, [isAuthenticated, router]);

  return (
    <div className="min-h-screen flex">
      {/* Sidebar */}
      <div className="w-64 bg-gray-800 text-white">
        <div className="p-4">
          <h1 className="text-2xl font-bold">HR Analytics</h1>
        </div>
        <nav className="mt-4">
          <a href="/dashboard" className="block p-4 hover:bg-gray-700">
            Dashboard
          </a>
          <a href="/employees" className="block p-4 hover:bg-gray-700">
            Employees
          </a>
          <a href="/departments" className="block p-4 hover:bg-gray-700">
            Departments
          </a>
        </nav>
      </div>

      {/* Main Content */}
      <div className="flex-1 bg-gray-100">
        <header className="bg-white shadow-sm">
          <div className="max-w-7xl mx-auto py-4 px-4">
            <h1 className="text-2xl font-semibold text-gray-900">Dashboard</h1>
          </div>
        </header>

        <main className="max-w-7xl mx-auto py-6 px-4">
          {children}
        </main>
      </div>
    </div>
  );
}