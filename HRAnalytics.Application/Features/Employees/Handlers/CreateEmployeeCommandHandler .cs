﻿using AutoMapper;
using HRAnalytics.Application.Common;
using HRAnalytics.Application.DTOs.Employee.Reguests;
using HRAnalytics.Application.Features.Employees.Commands;
using HRAnalytics.Core.Entities;
using HRAnalytics.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Application.Features.Employees.Handlers
{
    public class CreateEmployeeCommandHandler : BaseHandler, IRequestHandler<CreateEmployeeCommand, EmployeeDto>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public CreateEmployeeCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IEmployeeRepository employeeRepository) : base(unitOfWork, mapper)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = _mapper.Map<Employee>(request.Employee);

            await _employeeRepository.AddAsync(employee);
            await SaveChangesAsync();

            return _mapper.Map<EmployeeDto>(employee);
        }
    }

}
