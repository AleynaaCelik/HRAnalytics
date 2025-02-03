using HRAnalytics.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Core.Interfaces
{
  
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<IEnumerable<Employee>> GetAllWithDetailsAsync();
        Task<Employee?> GetByIdWithDetailsAsync(int id);
        Task<Employee?> GetEmployeeWithProgressAsync(int employeeId);

    }
}
