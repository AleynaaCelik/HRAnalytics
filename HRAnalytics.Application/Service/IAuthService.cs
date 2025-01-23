using HRAnalytics.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HRAnalytics.Application.Service
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}
