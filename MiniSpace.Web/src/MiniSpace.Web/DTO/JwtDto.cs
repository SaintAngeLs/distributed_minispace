namespace MiniSpace.Web.DTO
{
    public class JwtDto
    {
        public string AccessToken { get; set; }
        public string Role { get; set; }
        public string RefreshToken { get; set; }
        public long Expires { get; set; }
    }
}