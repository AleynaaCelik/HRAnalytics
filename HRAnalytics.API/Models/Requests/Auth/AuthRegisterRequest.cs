namespace HRAnalytics.API.Models.Requests.Auth
{
    public class AuthRegisterRequest
    {
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
