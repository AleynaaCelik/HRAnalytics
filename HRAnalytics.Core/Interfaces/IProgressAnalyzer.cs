using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Core.Interfaces
{
    public interface IProgressAnalyzer
    {
        Task<decimal> CalculateOverallProgressAsync(int employeeId);
        Task<IEnumerable<ProgressReport>> GetDepartmentProgressReportAsync(int departmentId);
    }

    public class ProgressReport
    {
        public string EmployeeName { get; set; } = default!;
        public decimal OverallProgress { get; set; }
        public int CompletedModules { get; set; }
        public int TotalModules { get; set; }
    }
}
