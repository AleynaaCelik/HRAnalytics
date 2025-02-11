// src/app/dashboard/page.tsx
'use client';

import MainLayout from '@/components/layout/MainLayout';
import { useEffect, useState } from 'react';
import api from '@/lib/axios';
import { 
  PieChart, Pie, Cell, ResponsiveContainer, Legend, Tooltip,
  LineChart, Line, XAxis, YAxis, CartesianGrid,
  BarChart, Bar
} from 'recharts';
import { DashboardStats, Activity, DepartmentData, EmployeeGrowthData, DepartmentCompletionData } from './types';

const COLORS = ['#0088FE', '#00C49F', '#FFBB28', '#FF8042', '#8884D8'];

export default function DashboardPage() {
  const [stats, setStats] = useState<DashboardStats>({
    totalEmployees: 0,
    totalDepartments: 0,
    completionRate: 0
  });
  const [activities, setActivities] = useState<Activity[]>([]);
  const [departmentData, setDepartmentData] = useState<DepartmentData[]>([]);
  const [employeeGrowth, setEmployeeGrowth] = useState<EmployeeGrowthData[]>([]);
  const [departmentCompletion, setDepartmentCompletion] = useState<DepartmentCompletionData[]>([]);
  const [loading, setLoading] = useState(true);
  const [timeFrame, setTimeFrame] = useState('month'); // 'month' | 'quarter' | 'year'

  useEffect(() => {
    const fetchDashboardData = async () => {
      try {
        setLoading(true);
        const [statsRes, activitiesRes, departmentsRes, growthRes, completionRes] = await Promise.all([
          api.get('/dashboard/stats'),
          api.get('/dashboard/activities'),
          api.get('/dashboard/departments'),
          api.get(`/dashboard/employee-growth?timeFrame=${timeFrame}`),
          api.get('/dashboard/department-completion')
        ]);

        setStats(statsRes.data.data);
        setActivities(activitiesRes.data.data);
        setDepartmentData(departmentsRes.data.data);
        setEmployeeGrowth(growthRes.data.data);
        setDepartmentCompletion(completionRes.data.data);
      } catch (error) {
        console.error('Error fetching dashboard data:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchDashboardData();
  }, [timeFrame]);

  return (
    <MainLayout>
      {/* Existing Stat Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 mb-8">
        {/* ... existing stat cards ... */}
      </div>

      {/* Time Frame Filter */}
      <div className="mb-6">
        <select 
          value={timeFrame}
          onChange={(e) => setTimeFrame(e.target.value)}
          className="p-2 border rounded-md"
        >
          <option value="month">Aylık</option>
          <option value="quarter">Çeyreklik</option>
          <option value="year">Yıllık</option>
        </select>
      </div>

      {/* Employee Growth Chart */}
      <div className="bg-white p-6 rounded-lg shadow mb-6">
        <h3 className="text-lg font-semibold text-gray-900 mb-4">Çalışan Artış Grafiği</h3>
        <div className="h-80">
          <ResponsiveContainer width="100%" height="100%">
            <LineChart data={employeeGrowth}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="month" />
              <YAxis />
              <Tooltip />
              <Legend />
              <Line type="monotone" dataKey="count" stroke="#8884d8" name="Çalışan Sayısı" />
            </LineChart>
          </ResponsiveContainer>
        </div>
      </div>

      {/* Department Charts Row */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mb-6">
        {/* Department Distribution Pie Chart */}
        <div className="bg-white p-6 rounded-lg shadow">
          <h3 className="text-lg font-semibold text-gray-900 mb-4">Departman Dağılımı</h3>
          <div className="h-80">
            <ResponsiveContainer width="100%" height="100%">
              <PieChart>
                <Pie
                  data={departmentData}
                  cx="50%"
                  cy="50%"
                  outerRadius={80}
                  fill="#8884d8"
                  dataKey="value"
                  label
                >
                  {departmentData.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                  ))}
                </Pie>
                <Tooltip />
                <Legend />
              </PieChart>
            </ResponsiveContainer>
          </div>
        </div>

        {/* Department Completion Bar Chart */}
        <div className="bg-white p-6 rounded-lg shadow">
          <h3 className="text-lg font-semibold text-gray-900 mb-4">Departman Bazlı Oryantasyon Tamamlama</h3>
          <div className="h-80">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart data={departmentCompletion}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="department" />
                <YAxis />
                <Tooltip />
                <Legend />
                <Bar dataKey="completionRate" fill="#8884d8" name="Tamamlama Oranı (%)" />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </div>
      </div>

      {/* Recent Activities Table */}
      <div className="bg-white p-6 rounded-lg shadow">
        {/* ... existing activities table ... */}
      </div>
    </MainLayout>
  );
}