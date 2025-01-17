using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Application.DTOs.Progress
{
    public class EmployeeProgressDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = default!;
        public decimal OverallProgress { get; set; }
        public List<ModuleProgressDto> ModuleProgress { get; set; } = new();
    }
}
