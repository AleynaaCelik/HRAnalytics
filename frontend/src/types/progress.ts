// src/types/progress.ts
export interface IProgressUpdate {
    moduleId: number;
    completionPercentage: number;
    status: 'NotStarted' | 'InProgress' | 'Completed';
  }
  
  export interface ProgressModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSubmit: (data: IProgressUpdate) => Promise<void>;
    currentProgress: {
      moduleId: number;
      moduleName: string;
      completionPercentage: number;
      status: 'NotStarted' | 'InProgress' | 'Completed';
    };
  }