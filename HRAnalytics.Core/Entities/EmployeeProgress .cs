using HRAnalytics.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Core.Entities
{
    public class EmployeeProgress : BaseEntity
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = default!;
        public int ModuleId { get; set; }
        public LearningModule Module { get; set; } = default!;
        public decimal CompletionPercentage { get; set; }
        public ProgressStatus Status { get; set; }
        public DateTime? CompletionDate { get; set; }
    }
}
