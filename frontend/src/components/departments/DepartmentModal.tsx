// src/components/departments/DepartmentModal.tsx
'use client';
import { useState, useEffect } from 'react';
// src/components/departments/DepartmentModal.tsx
import { DepartmentModalProps, IDepartment } from '@/types';

const defaultDepartment: IDepartment = {
  name: '',
  code: '',
  description: ''
};

export default function DepartmentModal({
  isOpen,
  onClose,
  onSubmit,
  department,
  mode
}: DepartmentModalProps) {
  const [formData, setFormData] = useState<IDepartment>(department || defaultDepartment);

  useEffect(() => {
    setFormData(department || defaultDepartment);
  }, [department]);

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
      <div className="bg-white rounded-lg p-8 max-w-md w-full">
        <h2 className="text-2xl font-bold mb-6">
          {mode === 'add' ? 'Yeni Departman Ekle' : 'Departman Düzenle'}
        </h2>

        <form onSubmit={(e) => {
          e.preventDefault();
          onSubmit(formData);
        }}>
          <div className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-700">Departman Adı</label>
              <input
                type="text"
                value={formData.name}
                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500"
                required
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700">Kod</label>
              <input
                type="text"
                value={formData.code}
                onChange={(e) => setFormData({ ...formData, code: e.target.value })}
                className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500"
                required
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700">Açıklama</label>
              <textarea
                value={formData.description}
                onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500"
                rows={3}
              />
            </div>
          </div>

          <div className="mt-6 flex justify-end space-x-3">
            <button
              type="button"
              onClick={onClose}
              className="px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50"
            >
              İptal
            </button>
            <button
              type="submit"
              className="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600"
            >
              {mode === 'add' ? 'Ekle' : 'Güncelle'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}