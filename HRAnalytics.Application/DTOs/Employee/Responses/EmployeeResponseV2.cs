using HRAnalytics.Application.DTOs.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Application.DTOs.Employee.Responses
{
    public class EmployeeResponseV2 : EmployeeResponse
    {
        public string FullName => $"{FirstName} {LastName}";
        public int TotalCompletedModules { get; set; }
        public decimal OverallProgress { get; set; }
        public List<ModuleProgressDto> ProgressDetails { get; set; } = new List<ModuleProgressDto>();
    }
}
