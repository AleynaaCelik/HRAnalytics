using HRAnalytics.Application.DTOs.Progress;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Application.Features.Employees.Commands
{
    public class UpdateEmployeeProgressCommand : IRequest<ModuleProgressDto>
    {
        public int EmployeeId { get; set; }
        public int ModuleId { get; set; }
        public decimal CompletionPercentage { get; set; }

        public UpdateEmployeeProgressCommand(int employeeId, int moduleId, decimal completionPercentage)
        {
            EmployeeId = employeeId;
            ModuleId = moduleId;
            CompletionPercentage = completionPercentage;
        }
    }
}
