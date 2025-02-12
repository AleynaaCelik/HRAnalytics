// app/employees/page.tsx
'use client';

import MainLayout from '@/components/layout/MainLayout';
import EmployeeModal from '@/components/employees/EmployeeModal';
import { IEmployee, IDepartment } from '@/components/employees/types';
import { useEffect, useState } from 'react';
import api from '@/lib/axios';

export default function EmployeesPage() {
  const [employees, setEmployees] = useState<IEmployee[]>([]);
  const [departments, setDepartments] = useState<IDepartment[]>([]);
  const [loading, setLoading] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedEmployee, setSelectedEmployee] = useState<IEmployee | undefined>(undefined);
  const [modalMode, setModalMode] = useState<'add' | 'edit'>('add');

  useEffect(() => {
    fetchEmployees();
    fetchDepartments();
  }, []);

  const fetchEmployees = async () => {
    try {
      setLoading(true);
      const response = await api.get('/employees');
      setEmployees(response.data.data);
    } catch (error) {
      console.error('Error fetching employees:', error);
    } finally {
      setLoading(false);
    }
  };

  const fetchDepartments = async () => {
    try {
      const response = await api.get('/departments');
      setDepartments(response.data.data);
    } catch (error) {
      console.error('Error fetching departments:', error);
    }
  };

  const handleModalSubmit = async (employeeData: IEmployee) => {
    try {
      if (modalMode === 'add') {
        await api.post('/employees', employeeData);
      } else {
        await api.put(`/employees/${selectedEmployee?.id}`, employeeData);
      }
      setIsModalOpen(false);
      fetchEmployees();
    } catch (error) {
      console.error('Error saving employee:', error);
    }
  };

  // ... Geri kalan kod aynı ...

  return (
    <MainLayout>
      {/* ... diğer kodlar ... */}
      
      <EmployeeModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSubmit={handleModalSubmit}
        employee={selectedEmployee}
        departments={departments}
        mode={modalMode}
      />
    </MainLayout>
  );
}