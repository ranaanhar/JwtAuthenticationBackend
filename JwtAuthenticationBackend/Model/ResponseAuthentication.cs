namespace JwtAuthenticationBackend.Model
{
    public class ResponseAuthentication
    {
        public string? Token { get; set; }
        public string? Expiration { get; set; }
        //TODO implement client side of refresh token
        public RefreshToken? RefreshToken { get; set; }

    }
}
