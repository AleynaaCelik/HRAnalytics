using HRAnalytics.Application.DTOs.Employee;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Application.Features.Employees.Queries
{
    public class GetAllEmployeesQuery : IRequest<IEnumerable<EmployeeDto>>
    {
    }
}
