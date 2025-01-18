using AutoMapper;
using HRAnalytics.Application.Common;
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

    public class DeleteEmployeeCommandHandler : BaseHandler, IRequestHandler<DeleteEmployeeCommand, bool>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public DeleteEmployeeCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IEmployeeRepository employeeRepository) : base(unitOfWork, mapper)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await GetEntityByIdAsync<Employee>(request.Id);

            employee.IsDeleted = true;
            await _employeeRepository.UpdateAsync(employee);
            await SaveChangesAsync();

            return true;
        }
    }

}
