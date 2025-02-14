// app/departments/page.tsx
'use client';

import MainLayout from '@/components/layout/MainLayout';
import DepartmentModal from '@/components/departments/DepartmentModal';
import { IDepartment, DepartmentModalProps } from '@/types/departments';
import { useEffect, useState } from 'react';
import api from '@/lib/axios';

export default function DepartmentsPage() {
  const [departments, setDepartments] = useState<IDepartment[]>([]);
  const [loading, setLoading] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedDepartment, setSelectedDepartment] = useState<IDepartment | undefined>(undefined);
  const [modalMode, setModalMode] = useState<'add' | 'edit'>('add');

  useEffect(() => {
    fetchDepartments();
  }, []);

  const fetchDepartments = async () => {
    try {
      setLoading(true);
      const response = await api.get('/departments');
      setDepartments(response.data.data);
    } catch (error) {
      console.error('Error fetching departments:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleModalSubmit = async (departmentData: IDepartment) => {
    try {
      if (modalMode === 'add') {
        await api.post('/departments', departmentData);
      } else {
        await api.put(`/departments/${selectedDepartment?.id}`, departmentData);
      }
      setIsModalOpen(false);
      fetchDepartments();
    } catch (error) {
      console.error('Error saving department:', error);
    }
  };

  const handleAddDepartment = () => {
    setModalMode('add');
    setSelectedDepartment(undefined);
    setIsModalOpen(true);
  };

  const handleEditDepartment = (department: IDepartment) => {
    setModalMode('edit');
    setSelectedDepartment(department);
    setIsModalOpen(true);
  };

  const handleDeleteDepartment = async (id: number) => {
    if (window.confirm('Bu departmanı silmek istediğinizden emin misiniz?')) {
      try {
        await api.delete(`/departments/${id}`);
        fetchDepartments();
      } catch (error) {
        console.error('Error deleting department:', error);
      }
    }
  };

  return (
    <MainLayout>
      <div className="bg-white shadow rounded-lg">
        <div className="p-6 border-b border-gray-200">
          <div className="flex justify-between items-center">
            <h2 className="text-xl font-semibold text-gray-800">Departmanlar</h2>
            <button
              className="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded-md"
              onClick={handleAddDepartment}
            >
              Yeni Departman Ekle
            </button>
          </div>
        </div>

        {loading ? (
          <div className="flex justify-center items-center h-64">
            <div className="animate-spin rounded-full h-10 w-10 border-b-2 border-blue-500"></div>
          </div>
        ) : (
          <div className="overflow-x-auto">
            <table className="min-w-full divide-y divide-gray-200">
              <thead className="bg-gray-50">
                <tr>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Departman Adı
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Kod
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Çalışan Sayısı
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Oluşturulma Tarihi
                  </th>
                  <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">
                    İşlemler
                  </th>
                </tr>
              </thead>
              <tbody className="bg-white divide-y divide-gray-200">
                {departments.map((department) => (
                  <tr key={department.id} className="hover:bg-gray-50">
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="text-sm font-medium text-gray-900">{department.name}</div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="text-sm text-gray-500">{department.code}</div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="text-sm text-gray-500">{department.employeeCount || 0}</div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="text-sm text-gray-500">
                        {department.createdDate && new Date(department.createdDate).toLocaleDateString('tr-TR')}
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                      <button
                        onClick={() => handleEditDepartment(department)}
                        className="text-indigo-600 hover:text-indigo-900 mr-4"
                      >
                        Düzenle
                      </button>
                      <button
                        onClick={() => handleDeleteDepartment(department.id!)}
                        className="text-red-600 hover:text-red-900"
                      >
                        Sil
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      <DepartmentModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSubmit={handleModalSubmit}
        department={selectedDepartment}
        mode={modalMode}
      />
    </MainLayout>
  );
}