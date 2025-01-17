using HRAnalytics.Application.DTOs.Progress;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Application.Features.Employees.Queries
{
    public class GetEmployeeProgressQuery : IRequest<EmployeeProgressDto>
    {
        public int EmployeeId { get; set; }

        public GetEmployeeProgressQuery(int employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
