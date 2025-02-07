using AutoMapper;
using HRAnalytics.Application.DTOs.Employee.Responses;
using HRAnalytics.Application.Service;
using HRAnalytics.Application.Settings;
using HRAnalytics.Core.Entities;
using HRAnalytics.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Tests.Services
{
    
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly AuthService _authService;
        private readonly JwtSettings _jwtSettings;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _jwtSettings = new JwtSettings
            {
                SecretKey = "your-test-secret-key-min-16-chars",
                Issuer = "test",
                Audience = "test",
                ExpiryInMinutes = 60
            };

            var options = Options.Create(_jwtSettings);
            _authService = new AuthService(options);
        }

        [Fact]
        public void GenerateJwtToken_ShouldReturnValidToken()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Email = "test@test.com",
                Roles = new List<string> { "Employee" }
            };

            // Act
            var token = _authService.GenerateJwtToken(user);

            // Assert
            token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void VerifyPassword_WithValidPassword_ShouldReturnTrue()
        {
            // Arrange
            var password = "Test123!";
            var hash = _authService.HashPassword(password);

            // Act
            var result = _authService.VerifyPassword(password, hash);

            // Assert
            result.Should().BeTrue();
        }
    }

    
}
