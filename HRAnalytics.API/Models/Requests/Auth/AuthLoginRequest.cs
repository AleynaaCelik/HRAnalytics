namespace HRAnalytics.API.Models.Requests.Auth
{
    public class AuthLoginRequest
    {
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
