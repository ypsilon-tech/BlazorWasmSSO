namespace IdentityServer.Models
{
    public class LoginRequest
    {
        public string RequestAction { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}