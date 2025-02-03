using HRAnalytics.Application.DTOs.Employee.Reguests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Application.Features.Employees.Commands
{
    public class UpdateEmployeeCommand : IRequest<EmployeeDto>
    {
        public int Id { get; set; }
        public UpdateEmployeeDto Employee { get; set; }

        public UpdateEmployeeCommand(int id, UpdateEmployeeDto employee)
        {
            Id = id;
            Employee = employee;
        }
    }

}
