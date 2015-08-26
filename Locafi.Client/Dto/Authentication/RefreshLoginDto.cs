namespace Locafi.Client.Model.Dto.Authentication
{
    public class RefreshLoginDto
    {
        public RefreshLoginDto(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
        public string RefreshToken { get; set; }
    }
}
