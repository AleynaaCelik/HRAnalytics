using AutoMapper;
using HRAnalytics.Application.DTOs.Employee.Responses;
using HRAnalytics.Core.Entities;
using HRAnalytics.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Tests.Controllers
{
    public class EmployeesControllerTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<EmployeesController>> _loggerMock;
        private readonly EmployeesController _controller;

        public EmployeesControllerTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<EmployeesController>>();

            _controller = new EmployeesController(
                _employeeRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllV1_ShouldReturnEmployeeList()
        {
            // Arrange
            var employees = new List<Employee>
        {
            new Employee { Id = 1, FirstName = "John", LastName = "Doe" },
            new Employee { Id = 2, FirstName = "Jane", LastName = "Doe" }
        };

            var employeeDtos = new List<EmployeeResponse>
        {
            new EmployeeResponse { Id = 1, FirstName = "John", LastName = "Doe" },
            new EmployeeResponse { Id = 2, FirstName = "Jane", LastName = "Doe" }
        };

            _employeeRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(employees);

            _mapperMock.Setup(x => x.Map<IEnumerable<EmployeeResponse>>(employees))
                .Returns(employeeDtos);

            // Act
            var result = await _controller.GetAllV1();

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ApiResponse<IEnumerable<EmployeeResponse>>>().Subject;
            response.Data.Should().HaveCount(2);
            response.Success.Should().BeTrue();
        }

        [Fact]
        public async Task GetByIdV1_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            _employeeRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Employee)null);

            // Act
            var result = await _controller.GetByIdV1(1);

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
