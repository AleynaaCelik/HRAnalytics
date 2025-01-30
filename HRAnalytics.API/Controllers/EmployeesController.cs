using AutoMapper;
using HRAnalytics.API.Models.Requests.Employee;
using HRAnalytics.API.Response;
using HRAnalytics.Application.DTOs.Employee;
using HRAnalytics.Application.DTOs.Progress;
using HRAnalytics.Application.Features.Employees.Commands;
using HRAnalytics.Application.Features.Employees.Queries;
using HRAnalytics.Core.Entities;
using HRAnalytics.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRAnalytics.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<EmployeesController> logger)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeeResponse>>>> GetAll()
        {
            var employees = await _employeeRepository.GetAllAsync();
            var response = _mapper.Map<IEnumerable<EmployeeResponse>>(employees);
            return Ok(ApiResponse<IEnumerable<EmployeeResponse>>.SuccessResult(response));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EmployeeResponse>>> GetById(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return NotFound(ApiResponse<EmployeeResponse>.FailureResult($"Employee with ID {id} not found"));

            var response = _mapper.Map<EmployeeResponse>(employee);
            return Ok(ApiResponse<EmployeeResponse>.SuccessResult(response));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<EmployeeResponse>>> Create([FromBody] CreateEmployeeRequest request)
        {
            var employee = _mapper.Map<Employee>(request);
            await _employeeRepository.AddAsync(employee);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<EmployeeResponse>(employee);
            return CreatedAtAction(nameof(GetById), new { id = employee.Id },
                ApiResponse<EmployeeResponse>.SuccessResult(response, "Employee created successfully"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<EmployeeResponse>>> Update(int id, [FromBody] UpdateEmployeeRequest request)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return NotFound(ApiResponse<EmployeeResponse>.FailureResult($"Employee with ID {id} not found"));

            _mapper.Map(request, employee);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<EmployeeResponse>(employee);
            return Ok(ApiResponse<EmployeeResponse>.SuccessResult(response, "Employee updated successfully"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return NotFound(ApiResponse<bool>.FailureResult($"Employee with ID {id} not found"));

            employee.IsDeleted = true;
            await _unitOfWork.SaveChangesAsync();

            return Ok(ApiResponse<bool>.SuccessResult(true, "Employee deleted successfully"));
        }
    }
}


