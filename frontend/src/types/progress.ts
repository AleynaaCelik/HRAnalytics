export interface IProgressUpdate {
  moduleId: number; // string yerine number olarak değiştirdik
  completionPercentage: number;
  status: 'NotStarted' | 'InProgress' | 'Completed';
}

export interface ProgressModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (data: IProgressUpdate) => void;
  currentProgress: {
    moduleId: number; // string yerine number olarak değiştirdik
    moduleName: string;
    completionPercentage: number;
  };
}