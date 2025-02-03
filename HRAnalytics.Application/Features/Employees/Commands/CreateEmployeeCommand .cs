using HRAnalytics.Application.DTOs.Employee.Reguests;
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
        public CreateEmployeeDto Employee { get; }

        public CreateEmployeeCommand(CreateEmployeeDto employee)
        {
            Employee = employee;
        }

    }
}
