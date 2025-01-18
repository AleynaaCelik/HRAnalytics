using HRAnalytics.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Application.DTOs
{
    public class LearningModuleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Content { get; set; }
        public int DurationInMinutes { get; set; }
        public ModuleType Type { get; set; }
    }
}
