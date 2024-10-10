namespace JwtAuthenticationBackend.Model
{
    public class ResponseAuthentication
    {
        public string? Token { get; set; }
        public string? Expiration { get; set; }
        public string? RefreshToken { get; set; }

    }
}
