// src/app/dashboard/types.ts
export type DashboardStats = {
    totalEmployees: number;
    totalDepartments: number;
    completionRate: number;
  };
  
  export type Activity = {
    id: number;
    type: string;
    description: string;
    date: string;
    user: string;
  };
  
  export type DepartmentData = {
    name: string;
    value: number;
  };
  
  export type EmployeeGrowthData = {
    month: string;
    count: number;
  };
  
  export type DepartmentCompletionData = {
    department: string;
    completionRate: number;
    totalEmployees: number;
  };