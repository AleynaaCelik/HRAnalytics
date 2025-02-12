// src/components/employees/types.ts
export interface IEmployee {
  id?: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  departmentId: number;
  departmentName?: string;
  hireDate: string;
}

export interface IDepartment {
  id: number;
  name: string;
}

export interface EmployeeModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (employeeData: IEmployee) => Promise<void>;
  employee: IEmployee | undefined;
  departments: IDepartment[];
  mode: 'add' | 'edit';
}