'use client';

import Link from 'next/link';
import { usePathname } from 'next/navigation';

export default function Header() {
  const pathname = usePathname();

  return (
    <header className="bg-white shadow">
      <nav className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between h-16">
          <div className="flex">
            <Link 
              href="/"
              className={`inline-flex items-center px-4 text-gray-900 ${
                pathname === '/' ? 'border-b-2 border-blue-500' : ''
              }`}
            >
              Dashboard
            </Link>
            <Link 
              href="/employees"
              className={`inline-flex items-center px-4 text-gray-900 ${
                pathname.startsWith('/employees') ? 'border-b-2 border-blue-500' : ''
              }`}
            >
              Çalışanlar
            </Link>
            <Link 
              href="/departments"
              className={`inline-flex items-center px-4 text-gray-900 ${
                pathname.startsWith('/departments') ? 'border-b-2 border-blue-500' : ''
              }`}
            >
              Departmanlar
            </Link>
          </div>
        </div>
      </nav>
    </header>
  );
}