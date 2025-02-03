using HRAnalytics.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Core.Entities
{
    public class Employee : AuditableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<EmployeeProgress> ProgressRecords { get; set; } = new List<EmployeeProgress>();

    }
}
