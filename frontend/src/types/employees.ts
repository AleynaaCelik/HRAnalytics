// src/types/employees.ts
import { IDepartment } from './departments';

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

export interface IEmployeeDetail {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  hireDate: string;
  department: IDepartment;
  progressRecords: {
    moduleId: number;
    moduleName: string;
    completionPercentage: number;
    completionDate?: string;
    status: 'NotStarted' | 'InProgress' | 'Completed';
  }[];
  totalProgress: number;
}

export interface IProgressUpdate {
  moduleId: string;
  completionPercentage: number;
  status: 'NotStarted' | 'InProgress' | 'Completed';
  completionDate?: Date;
}