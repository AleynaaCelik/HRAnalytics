using AutoMapper;
using HRAnalytics.API.Controllers;
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
        public async Task GetById_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            int testId = 1;
            _employeeRepositoryMock.Setup(x => x.GetByIdAsync(testId))
                .ReturnsAsync((Employee)null);

            // Act
            var result = await _controller.GetByIdV1(testId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}
