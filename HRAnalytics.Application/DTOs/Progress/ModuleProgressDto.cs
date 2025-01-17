using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Application.DTOs.Progress
{
    public class ModuleProgressDto
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = default!;
        public decimal CompletionPercentage { get; set; }
        public string Status { get; set; } = default!;
        public DateTime? CompletionDate { get; set; }
    }
}
