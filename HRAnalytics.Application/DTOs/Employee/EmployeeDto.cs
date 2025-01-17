using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Application.DTOs.Employee
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = default!;

    }
}
