// src/components/employees/ProgressModal.tsx
'use client';

import { useState } from 'react';
import { ProgressModalProps } from '@/types/progress';

export default function ProgressModal({
  isOpen,
  onClose,
  onSubmit,
  currentProgress
}: ProgressModalProps) {
  const [percentage, setPercentage] = useState(currentProgress.completionPercentage);

  if (!isOpen) return null;

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmit({
      moduleId: currentProgress.moduleId,
      completionPercentage: percentage,
      status: percentage === 100 ? 'Completed' : percentage > 0 ? 'InProgress' : 'NotStarted'
    });
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
      <div className="bg-white rounded-lg p-8 max-w-md w-full">
        <h2 className="text-2xl font-bold mb-6">İlerleme Durumu Güncelle</h2>
        <h3 className="text-lg text-gray-700 mb-4">{currentProgress.moduleName}</h3>

        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Tamamlanma Yüzdesi
            </label>
            <input
              type="range"
              min="0"
              max="100"
              value={percentage}
              onChange={(e) => setPercentage(Number(e.target.value))}
              className="w-full"
            />
            <div className="text-center text-lg font-semibold mt-2">
              {percentage}%
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
              Güncelle
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}