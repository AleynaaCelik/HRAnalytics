using HRAnalytics.Application.Service;
using HRAnalytics.Core.Entities;
using HRAnalytics.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HRAnalytics.API.Response;
using HRAnalytics.API.Models.Requests.Auth;

namespace HRAnalytics.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] AuthLoginRequest request)
        {
            try
            {
                _logger.LogInformation("Login attempt for user: {Username}", request.Username);

                var user = await _userRepository.GetByUsernameAsync(request.Username);
                if (user == null || !_authService.VerifyPassword(request.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Invalid login attempt for user: {Username}", request.Username);
                    return Unauthorized(ApiResponse<AuthResponse>.FailureResult("Invalid credentials"));
                }

                var token = _authService.GenerateJwtToken(user);
                var refreshToken = _authService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
                await _unitOfWork.SaveChangesAsync();

                var response = new AuthResponse
                {
                    AccessToken = token,
                    RefreshToken = refreshToken,
                    Username = user.Username,
                    Email = user.Email,
                    Roles = user.Roles
                };

                _logger.LogInformation("User {Username} logged in successfully", user.Username);
                return Ok(ApiResponse<AuthResponse>.SuccessResult(response, "Login successful"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user: {Username}", request.Username);
                return StatusCode(500, ApiResponse<AuthResponse>.FailureResult("An error occurred during login"));
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> RefreshToken([FromBody] AuthRefreshTokenRequest request)
        {
            try
            {
                var principal = _authService.GetPrincipalFromExpiredToken(request.AccessToken);
                if (principal == null)
                {
                    return BadRequest(ApiResponse<AuthResponse>.FailureResult("Invalid access token"));
                }

                var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var user = await _userRepository.GetByIdAsync(userId);

                if (user == null ||
                    user.RefreshToken != request.RefreshToken ||
                    user.RefreshTokenExpiry <= DateTime.UtcNow)
                {
                    return BadRequest(ApiResponse<AuthResponse>.FailureResult("Invalid refresh token"));
                }

                var newToken = _authService.GenerateJwtToken(user);
                var newRefreshToken = _authService.GenerateRefreshToken();

                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
                await _unitOfWork.SaveChangesAsync();

                var response = new AuthResponse
                {
                    AccessToken = newToken,
                    RefreshToken = newRefreshToken,
                    Username = user.Username,
                    Email = user.Email,
                    Roles = user.Roles
                };

                return Ok(ApiResponse<AuthResponse>.SuccessResult(response, "Token refreshed successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return StatusCode(500, ApiResponse<AuthResponse>.FailureResult("An error occurred while refreshing token"));
            }
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public async Task<ActionResult<ApiResponse<bool>>> RevokeToken()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var user = await _userRepository.GetByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(ApiResponse<bool>.FailureResult("User not found"));
                }

                user.RefreshToken = null;
                user.RefreshTokenExpiry = null;
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Token revoked for user: {Username}", user.Username);
                return Ok(ApiResponse<bool>.SuccessResult(true, "Token revoked successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token revocation");
                return StatusCode(500, ApiResponse<bool>.FailureResult("An error occurred while revoking token"));
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> Register([FromBody] AuthRegisterRequest request)
        {
            try
            {
                if (await _userRepository.IsUsernameUniqueAsync(request.Username))
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

                _logger.LogInformation("New user registered: {Username}", user.Username);
                return Ok(ApiResponse<AuthResponse>.SuccessResult(response, "Registration successful"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration: {Username}", request.Username);
                return StatusCode(500, ApiResponse<AuthResponse>.FailureResult("An error occurred during registration"));
            }
        }
    }
}