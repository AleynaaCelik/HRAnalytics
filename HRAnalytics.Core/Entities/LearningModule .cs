using HRAnalytics.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Core.Entities
{
    public class LearningModule : AuditableEntity
    {
        public string Name { get; set; } = default!;
        public string? Content { get; set; }
        public int DurationInMinutes { get; set; }
        public ModuleType Type { get; set; }
        public ICollection<EmployeeProgress> ProgressRecords { get; set; } = new List<EmployeeProgress>();
    }
}
