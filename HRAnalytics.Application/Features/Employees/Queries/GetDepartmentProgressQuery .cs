using HRAnalytics.Application.DTOs.Progress;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Application.Features.Employees.Queries
{
    public class GetDepartmentProgressQuery : IRequest<IEnumerable<EmployeeProgressDto>>
    {
        public int DepartmentId { get; set; }

        public GetDepartmentProgressQuery(int departmentId)
        {
            DepartmentId = departmentId;
        }
    }
}
