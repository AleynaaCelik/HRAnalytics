using HRAnalytics.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Core.Entities
{
    public class Department : AuditableEntity
    {
        public string Name { get; set; } = default!;
        public string? Code { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    
    }
}
