using HRAnalytics.Application.DTOs.Employee;
using HRAnalytics.Application.DTOs.Progress;
using HRAnalytics.Application.Features.Employees.Commands;
using HRAnalytics.Application.Features.Employees.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRAnalytics.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllEmployeesQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetById(int id)
        {
            var result = await _mediator.Send(new GetEmployeeByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> Create([FromBody] CreateEmployeeDto employeeDto)
        {
            var command = new CreateEmployeeCommand(employeeDto);
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeDto>> Update(int id, [FromBody] UpdateEmployeeDto employeeDto)
        {
            var command = new UpdateEmployeeCommand(id, employeeDto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var command = new DeleteEmployeeCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}/progress")]
        public async Task<ActionResult<EmployeeProgressDto>> GetProgress(int id)
        {
            var result = await _mediator.Send(new GetEmployeeProgressQuery(id));
            return Ok(result);
        }

        [HttpPut("{employeeId}/progress/{moduleId}")]
        public async Task<ActionResult<ModuleProgressDto>> UpdateProgress(
            int employeeId,
            int moduleId,
            [FromBody] decimal completionPercentage)
        {
            var command = new UpdateEmployeeProgressCommand(employeeId, moduleId, completionPercentage);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

