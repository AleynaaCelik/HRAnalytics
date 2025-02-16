'use client';

import MainLayout from '@/components/layout/MainLayout';
import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import api from '@/lib/axios';
import { IEmployeeDetail } from '@/types/employees';
import ProgressModal from '@/components/employees/ProgressModal';
import { IProgressUpdate } from '@/types/progress';

export default function EmployeeDetailPage({ params }: { params: { id: string } }) {
  const [employee, setEmployee] = useState<IEmployeeDetail | null>(null);
  const [loading, setLoading] = useState(true);
  const [isProgressModalOpen, setIsProgressModalOpen] = useState(false);
  const [selectedProgress, setSelectedProgress] = useState<any>(null);
  const router = useRouter();

  useEffect(() => {
    fetchEmployeeDetails();
  }, [params.id]);

  const fetchEmployeeDetails = async () => {
    try {
      setLoading(true);
      const response = await api.get(`/employees/${params.id}`);
      setEmployee(response.data.data);
    } catch (error) {
      console.error('Error fetching employee details:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleProgressUpdate = async (data: IProgressUpdate) => {
    try {
      await api.put(`/employees/${params.id}/progress`, data);
      await fetchEmployeeDetails();
      setIsProgressModalOpen(false);
    } catch (error) {
      console.error('Error updating progress:', error);
    }
  };

  if (loading) {
    return (
      <MainLayout>
        <div className="flex justify-center items-center h-64">
          <div className="animate-spin rounded-full h-10 w-10 border-b-2 border-blue-500"></div>
        </div>
      </MainLayout>
    );
  }

  if (!employee) {
    return (
      <MainLayout>
        <div className="text-center py-10">
          <h2 className="text-2xl font-semibold text-gray-900">Çalışan bulunamadı</h2>
          <button
            onClick={() => router.back()}
            className="mt-4 text-blue-600 hover:text-blue-800"
          >
            Geri Dön
          </button>
        </div>
      </MainLayout>
    );
  }

  return (
    <MainLayout>
      <div className="bg-white shadow rounded-lg">
        {/* Header Section */}
        <div className="p-6 border-b border-gray-200">
          <div className="flex justify-between items-center">
            <h2 className="text-2xl font-bold text-gray-900">
              {employee.firstName} {employee.lastName}
            </h2>
            <button
              onClick={() => router.back()}
              className="text-gray-600 hover:text-gray-900"
            >
              Geri Dön
            </button>
          </div>
        </div>

        {/* Basic Info Section */}
        <div className="p-6 border-b border-gray-200">
          <h3 className="text-lg font-semibold mb-4">Temel Bilgiler</h3>
          <div className="grid grid-cols-2 gap-4">
            <div>
              <p className="text-sm text-gray-600">Email</p>
              <p className="text-gray-900">{employee.email}</p>
            </div>
            <div>
              <p className="text-sm text-gray-600">Telefon</p>
              <p className="text-gray-900">{employee.phoneNumber}</p>
            </div>
            <div>
              <p className="text-sm text-gray-600">Departman</p>
              <p className="text-gray-900">{employee.department.name}</p>
            </div>
            <div>
              <p className="text-sm text-gray-600">İşe Giriş Tarihi</p>
              <p className="text-gray-900">
                {new Date(employee.hireDate).toLocaleDateString('tr-TR')}
              </p>
            </div>
          </div>
        </div>

        {/* Progress Section */}
        <div className="p-6">
          <h3 className="text-lg font-semibold mb-4">Oryantasyon İlerlemesi</h3>
          <div className="mb-4">
            <p className="text-sm text-gray-600">Genel İlerleme</p>
            <div className="w-full h-2 bg-gray-200 rounded-full mt-2">
              <div
                className="h-full bg-blue-500 rounded-full"
                style={{ width: `${employee.totalProgress}%` }}
              />
            </div>
            <p className="text-right text-sm text-gray-600 mt-1">
              {employee.totalProgress}%
            </p>
          </div>

          <div className="space-y-4">
            {employee.progressRecords.map((record) => (
              <div key={record.moduleId} className="border rounded-lg p-4">
                <div className="flex justify-between items-center mb-2">
                  <h4 className="font-medium">{record.moduleName}</h4>
                  <span className={`px-2 py-1 rounded text-sm ${
                    record.status === 'Completed' ? 'bg-green-100 text-green-800' :
                    record.status === 'InProgress' ? 'bg-yellow-100 text-yellow-800' :
                    'bg-gray-100 text-gray-800'
                  }`}>
                    {record.status === 'Completed' ? 'Tamamlandı' :
                    record.status === 'InProgress' ? 'Devam Ediyor' :
                    'Başlanmadı'}
                  </span>
                </div>
                <div className="w-full h-2 bg-gray-200 rounded-full">
                  <div
                    className="h-full bg-blue-500 rounded-full"
                    style={{ width: `${record.completionPercentage}%` }}
                  />
                </div>
                <div className="flex justify-between mt-1">
                  <span className="text-sm text-gray-600">{record.completionPercentage}%</span>
                  {record.completionDate && (
                    <span className="text-sm text-gray-600">
                      Tamamlanma: {new Date(record.completionDate).toLocaleDateString('tr-TR')}
                    </span>
                  )}
                </div>
                <button
                  onClick={() => {
                    setSelectedProgress(record);
                    setIsProgressModalOpen(true);
                  }}
                  className="mt-2 text-sm text-blue-600 hover:text-blue-800"
                >
                  İlerlemeyi Güncelle
                </button>
              </div>
            ))}
          </div>
        </div>
      </div>

      {/* Progress Update Modal */}
      {selectedProgress && (
        <ProgressModal
          isOpen={isProgressModalOpen}
          onClose={() => setIsProgressModalOpen(false)}
          onSubmit={handleProgressUpdate}
          currentProgress={selectedProgress}
        />
      )}
    </MainLayout>
  );
}
