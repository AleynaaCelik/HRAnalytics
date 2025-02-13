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
 const [searchTerm, setSearchTerm] = useState('');
 const [selectedDepartment, setSelectedDepartment] = useState<number | ''>('');
 const [sortBy, setSortBy] = useState<'name' | 'department' | 'hireDate'>('name');
 const [sortOrder, setSortOrder] = useState<'asc' | 'desc'>('asc');

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

 const handleAddEmployee = () => {
   setModalMode('add');
   setSelectedEmployee(undefined);
   setIsModalOpen(true);
 };

 const handleEditEmployee = (employee: IEmployee) => {
   setModalMode('edit');
   setSelectedEmployee(employee);
   setIsModalOpen(true);
 };

 const handleDeleteEmployee = async (id: number) => {
   if (window.confirm('Bu çalışanı silmek istediğinizden emin misiniz?')) {
     try {
       await api.delete(`/employees/${id}`);
       fetchEmployees();
     } catch (error) {
       console.error('Error deleting employee:', error);
     }
   }
 };

 const filteredEmployees = employees
   .filter(employee => {
     const matchesSearch = 
       employee.firstName.toLowerCase().includes(searchTerm.toLowerCase()) ||
       employee.lastName.toLowerCase().includes(searchTerm.toLowerCase()) ||
       employee.email.toLowerCase().includes(searchTerm.toLowerCase());
     
     const matchesDepartment = selectedDepartment === '' || employee.departmentId === selectedDepartment;
     
     return matchesSearch && matchesDepartment;
   })
   .sort((a, b) => {
     if (sortBy === 'name') {
       const nameA = `${a.firstName} ${a.lastName}`;
       const nameB = `${b.firstName} ${b.lastName}`;
       return sortOrder === 'asc' ? nameA.localeCompare(nameB) : nameB.localeCompare(nameA);
     }
     if (sortBy === 'department') {
       return sortOrder === 'asc' 
         ? (a.departmentName || '').localeCompare(b.departmentName || '')
         : (b.departmentName || '').localeCompare(a.departmentName || '');
     }
     return sortOrder === 'asc'
       ? new Date(a.hireDate).getTime() - new Date(b.hireDate).getTime()
       : new Date(b.hireDate).getTime() - new Date(a.hireDate).getTime();
   });

 return (
   <MainLayout>
     <div className="bg-white shadow rounded-lg">
       <div className="p-6 border-b border-gray-200">
         <div className="flex justify-between items-center">
           <h2 className="text-xl font-semibold text-gray-800">Çalışanlar</h2>
           <button
             className="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded-md"
             onClick={handleAddEmployee}
           >
             Yeni Çalışan Ekle
           </button>
         </div>
       </div>

       <div className="p-4 bg-white border-b border-gray-200">
         <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
           <div>
             <label className="block text-sm font-medium text-gray-700">Arama</label>
             <input
               type="text"
               value={searchTerm}
               onChange={(e) => setSearchTerm(e.target.value)}
               placeholder="Ad, soyad veya email ile ara..."
               className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500"
             />
           </div>

           <div>
             <label className="block text-sm font-medium text-gray-700">Departman</label>
             <select
               value={selectedDepartment}
               onChange={(e) => setSelectedDepartment(e.target.value ? Number(e.target.value) : '')}
               className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500"
             >
               <option value="">Tümü</option>
               {departments.map((dept) => (
                 <option key={dept.id} value={dept.id}>
                   {dept.name}
                 </option>
               ))}
             </select>
           </div>

           <div>
             <label className="block text-sm font-medium text-gray-700">Sıralama</label>
             <div className="mt-1 flex space-x-2">
               <select
                 value={sortBy}
                 onChange={(e) => setSortBy(e.target.value as 'name' | 'department' | 'hireDate')}
                 className="block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500"
               >
                 <option value="name">İsim</option>
                 <option value="department">Departman</option>
                 <option value="hireDate">İşe Giriş Tarihi</option>
               </select>
               <button
                 onClick={() => setSortOrder(sortOrder === 'asc' ? 'desc' : 'asc')}
                 className="px-3 py-2 border border-gray-300 rounded-md"
               >
                 {sortOrder === 'asc' ? '↑' : '↓'}
               </button>
             </div>
           </div>
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
                   Ad Soyad
                 </th>
                 <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                   Email
                 </th>
                 <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                   Departman
                 </th>
                 <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                   İşe Giriş Tarihi
                 </th>
                 <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">
                   İşlemler
                 </th>
               </tr>
             </thead>
             <tbody className="bg-white divide-y divide-gray-200">
               {filteredEmployees.map((employee) => (
                 <tr key={employee.id} className="hover:bg-gray-50">
                   <td className="px-6 py-4 whitespace-nowrap">
                     <div className="text-sm font-medium text-gray-900">
                       {employee.firstName} {employee.lastName}
                     </div>
                   </td>
                   <td className="px-6 py-4 whitespace-nowrap">
                     <div className="text-sm text-gray-500">{employee.email}</div>
                   </td>
                   <td className="px-6 py-4 whitespace-nowrap">
                     <div className="text-sm text-gray-500">{employee.departmentName}</div>
                   </td>
                   <td className="px-6 py-4 whitespace-nowrap">
                     <div className="text-sm text-gray-500">
                       {new Date(employee.hireDate).toLocaleDateString('tr-TR')}
                     </div>
                   </td>
                   <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                     <button
                       onClick={() => handleEditEmployee(employee)}
                       className="text-indigo-600 hover:text-indigo-900 mr-4"
                     >
                       Düzenle
                     </button>
                     <button
                       onClick={() => handleDeleteEmployee(employee.id!)}
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