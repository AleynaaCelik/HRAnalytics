using AutoMapper;
using HRAnalytics.Application.Common;
using HRAnalytics.Application.DTOs.Progress;
using HRAnalytics.Application.Features.Employees.Commands;
using HRAnalytics.Core.Entities;
using HRAnalytics.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRAnalytics.Core.Enums;

namespace HRAnalytics.Application.Features.Employees.Handlers
{
    public class UpdateEmployeeProgressCommandHandler : BaseHandler, IRequestHandler<UpdateEmployeeProgressCommand, ModuleProgressDto>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public UpdateEmployeeProgressCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IEmployeeRepository employeeRepository) : base(unitOfWork, mapper)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<ModuleProgressDto> Handle(UpdateEmployeeProgressCommand request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetEmployeeWithProgressAsync(request.EmployeeId);
            if (employee == null)
                throw new NotFoundException(nameof(Employee), request.EmployeeId);

            var progress = employee.ProgressRecords
                .FirstOrDefault(p => p.ModuleId == request.ModuleId);

            if (progress == null)
            {
                progress = new EmployeeProgress
                {
                    EmployeeId = request.EmployeeId,
                    ModuleId = request.ModuleId,
                    CompletionPercentage = request.CompletionPercentage,
                    Status = request.CompletionPercentage >= 100 ? ProgressStatus.Completed : ProgressStatus.InProgress
                };
                await _unitOfWork.Repository<EmployeeProgress>().AddAsync(progress);
            }
            else
            {
                progress.CompletionPercentage = request.CompletionPercentage;
                progress.Status = request.CompletionPercentage >= 100 ? ProgressStatus.Completed : ProgressStatus.InProgress;
                progress.CompletionDate = request.CompletionPercentage >= 100 ? DateTime.UtcNow : null;
            }

            await SaveChangesAsync();

            return _mapper.Map<ModuleProgressDto>(progress);
        }
    }
}

