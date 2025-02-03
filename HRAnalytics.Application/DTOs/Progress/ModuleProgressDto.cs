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
        public string ModuleName { get; set; }
        public decimal CompletionPercentage { get; set; }
        public DateTime? CompletionDate { get; set; }
    }
}
