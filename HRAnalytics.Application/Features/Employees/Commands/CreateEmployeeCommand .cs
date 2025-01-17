using HRAnalytics.Application.DTOs.Employee;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Application.Features.Employees.Commands
{
    public class CreateEmployeeCommand : IRequest<EmployeeDto>
    {
        public CreateEmployeeDto Employee { get; set; }

        public CreateEmployeeCommand(CreateEmployeeDto employee)
        {
            Employee = employee;
        }
    }
}
