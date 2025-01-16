using HRAnalytics.Core.Entities;
using HRAnalytics.Core.Interfaces;
using HRAnalytics.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Infrastructure.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int departmentId)
        {
            return await _context.Employees
                .Where(e => e.DepartmentId == departmentId && !e.IsDeleted)
                .Include(e => e.Department)
                .ToListAsync();
        }

        public async Task<Employee?> GetEmployeeWithProgressAsync(int employeeId)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.ProgressRecords)
                    .ThenInclude(p => p.Module)
                .FirstOrDefaultAsync(e => e.Id == employeeId && !e.IsDeleted);
        }
    }
}
