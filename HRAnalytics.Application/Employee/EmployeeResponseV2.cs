using HRAnalytics.API.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Application.Employee
{
    public class EmployeeResponseV2 : EmployeeResponse
    {
        public string FullName => $"{FirstName} {LastName}";
        public int TotalCompletedModules { get; set; }
        public decimal OverallProgress { get; set; }
        public List<EmployeeProgressResponse> ProgressDetails { get; set; }
    }
}
