using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Core.Common
{
    public abstract class AuditableEntity:BaseEntity
    {
        public string? Notes { get; set; }
        public string? Description { get; set; }
    }
}
