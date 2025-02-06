using AutoMapper;
using HRAnalytics.API.Models.Requests.Employee;
using HRAnalytics.API.Response;
using HRAnalytics.Application.DTOs.Employee;
using HRAnalytics.Application.DTOs.Employee.Responses;
using HRAnalytics.Application.DTOs.Progress;
using HRAnalytics.Core.Entities;
using HRAnalytics.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRAnalytics.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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
        [MapToApiVersion("1.0")]
        [Authorize(Policy = "AllEmployees")]
        [ResponseCache(Duration = 60)] // 60 saniye cache

        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeeResponse>>>> GetAllV1()
        {
            var employees = await _employeeRepository.GetAllAsync();
            var response = _mapper.Map<IEnumerable<EmployeeResponse>>(employees);
            return Ok(ApiResponse<IEnumerable<EmployeeResponse>>.SuccessResult(response));
        }

        [HttpGet]
        [MapToApiVersion("2.0")]
        [Authorize(Policy = "AllEmployees")]
        [ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "id" })]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeeResponseV2>>>> GetAllV2()
        {
            var employees = await _employeeRepository.GetAllWithDetailsAsync();
            var response = _mapper.Map<IEnumerable<EmployeeResponseV2>>(employees);
            return Ok(ApiResponse<IEnumerable<EmployeeResponseV2>>.SuccessResult(response));
        }

        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Policy = "AllEmployees")]
        public async Task<ActionResult<ApiResponse<EmployeeResponse>>> GetByIdV1(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return NotFound(ApiResponse<EmployeeResponse>.FailureResult($"Employee with ID {id} not found"));

            var response = _mapper.Map<EmployeeResponse>(employee);
            return Ok(ApiResponse<EmployeeResponse>.SuccessResult(response));
        }

        [HttpGet("{id}")]
        [MapToApiVersion("2.0")]
        [Authorize(Policy = "AllEmployees")]
        public async Task<ActionResult<ApiResponse<EmployeeResponseV2>>> GetByIdV2(int id)
        {
            var employee = await _employeeRepository.GetByIdWithDetailsAsync(id);
            if (employee == null)
                return NotFound(ApiResponse<EmployeeResponseV2>.FailureResult($"Employee with ID {id} not found"));

            var response = _mapper.Map<EmployeeResponseV2>(employee);
            return Ok(ApiResponse<EmployeeResponseV2>.SuccessResult(response));
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        [Authorize(Policy = "RequireManagerRole")]
        public async Task<ActionResult<ApiResponse<EmployeeResponse>>> Create([FromBody] CreateEmployeeRequest request)
        {
            var employee = _mapper.Map<Employee>(request);
            await _employeeRepository.AddAsync(employee);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<EmployeeResponse>(employee);
            return CreatedAtAction(nameof(GetByIdV1), new { id = employee.Id, version = "1.0" },
                ApiResponse<EmployeeResponse>.SuccessResult(response, "Employee created successfully"));
        }

        [HttpPut("{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Policy = "RequireManagerRole")]
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
        [MapToApiVersion("1.0")]
        [Authorize(Policy = "RequireAdminRole")]
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