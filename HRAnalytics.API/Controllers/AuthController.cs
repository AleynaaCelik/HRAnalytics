using HRAnalytics.Application.Service;
using HRAnalytics.Core.Entities;
using HRAnalytics.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HRAnalytics.API.Response;
using HRAnalytics.API.Models.Requests.Auth;

namespace HRAnalytics.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        private readonly IUnitOfWork _unitOfWork;

        public AuthController(
            IAuthService authService,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] AuthLoginRequest request)
        {
            var user = await _userRepository.GetUserByUsernameAsync(request.Username);
            if (user == null || !_authService.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized(ApiResponse<AuthResponse>.FailureResult("Invalid credentials"));
            }

            var token = _authService.GenerateJwtToken(user);
            var refreshToken = _authService.GenerateRefreshToken();

            var response = new AuthResponse
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                Username = user.Username,
                Email = user.Email,
                Roles = user.Roles
            };

            return Ok(ApiResponse<AuthResponse>.SuccessResult(response, "Login successful"));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] AuthRefreshTokenRequest request)
        {
            var principal = _authService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                return BadRequest("Geçersiz access token");
            }

            var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null ||
                user.RefreshToken != request.RefreshToken ||
                user.RefreshTokenExpiry <= DateTime.UtcNow)
            {
                return BadRequest("Geçersiz refresh token");
            }

            var newToken = _authService.GenerateJwtToken(user);
            var newRefreshToken = _authService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.SaveChangesAsync();

            return Ok(new
            {
                AccessToken = newToken,
                RefreshToken = newRefreshToken
            });
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;

            await _unitOfWork.SaveChangesAsync();

            return Ok(new { message = "Token başarıyla iptal edildi" });
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> Register([FromBody] AuthRegisterRequest request)
        {
            if (await _userRepository.UsernameExistsAsync(request.Username))
            {
                return BadRequest(ApiResponse<AuthResponse>.FailureResult("Username already exists"));
            }

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = _authService.HashPassword(request.Password),
                Roles = new List<string> { Role.Employee }
            };

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var response = new AuthResponse
            {
                Username = user.Username,
                Email = user.Email,
                Roles = user.Roles
            };

            return Ok(ApiResponse<AuthResponse>.SuccessResult(response, "Registration successful"));
        }
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}

