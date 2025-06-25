namespace AngularAuthAPI.Models.Dto
{
    public class TokenApiDTO
    {
        public string accessToken { get; set; } = string.Empty;
        public string refreshToken { get; set; } = string.Empty;
    }
}
