// src/types/index.ts
export interface IDepartment {
    id?: number;
    name: string;
    code: string;
    description?: string;
    employeeCount?: number;
    createdDate?: string;
    modifiedDate?: string;
  }
  
  export interface DepartmentModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSubmit: (departmentData: IDepartment) => Promise<void>;
    department: IDepartment | undefined;
    mode: 'add' | 'edit';
  }