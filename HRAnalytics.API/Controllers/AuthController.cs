using HRAnalytics.API.Models.Requests;
using HRAnalytics.Application.Service;
using HRAnalytics.Core.Entities;
using HRAnalytics.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HRAnalytics.API.Models.Requests;

namespace HRAnalytics.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(
            IAuthService authService,
            IRepository<User> userRepository,
            IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginRequest request)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Username == request.Username);

            if (user == null || !_authService.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Kullanıcı adı veya şifre hatalı" });
            }

            var token = _authService.GenerateJwtToken(user);
            var refreshToken = _authService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.SaveChangesAsync();

            return Ok(new
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                Username = user.Username,
                Email = user.Email,
                Roles = user.Roles
            });
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
        public async Task<IActionResult> Register([FromBody] AuthRegisterRequest request)
        {
            var users = await _userRepository.GetAllAsync();
            if (users.Any(u => u.Username == request.Username || u.Email == request.Email))
            {
                return BadRequest("Kullanıcı adı veya email zaten kullanımda");
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

            return Ok(new { message = "Kullanıcı başarıyla oluşturuldu" });
        }
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}

