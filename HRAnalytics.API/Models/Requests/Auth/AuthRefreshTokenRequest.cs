namespace HRAnalytics.API.Models.Requests.Auth
{
    public class AuthRefreshTokenRequest
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
    }
}
