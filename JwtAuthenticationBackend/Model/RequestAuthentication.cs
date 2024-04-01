namespace JwtAuthenticationBackend.Model
{
    public class RequestAuthentication
    {
        public string? UsernameOrEmail { get; set; }
        public string? Password { get; set; }
    }
}
