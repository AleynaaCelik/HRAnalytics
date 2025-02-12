// src/components/employees/EmployeeModal.tsx
'use client';

import { useState, useEffect } from 'react';
import { EmployeeModalProps, IEmployee } from './types';

const defaultEmployee: IEmployee = {
  firstName: '',
  lastName: '',
  email: '',
  phoneNumber: '',
  departmentId: 0,
  hireDate: ''
};

export default function EmployeeModal({
  isOpen,
  onClose,
  onSubmit,
  employee,
  departments,
  mode
}: EmployeeModalProps) {
  const [formData, setFormData] = useState<IEmployee>(employee || defaultEmployee);

  useEffect(() => {
    setFormData(employee || defaultEmployee);
  }, [employee]);

  if (!isOpen) return null;

  return (
    <div>
      {/* Modal içeriği */}
    </div>
  );
}