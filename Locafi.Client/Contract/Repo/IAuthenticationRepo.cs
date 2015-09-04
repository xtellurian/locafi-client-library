using System.Threading.Tasks;
using Locafi.Client.Model.Dto;
using Locafi.Client.Model.Dto.Authentication;

namespace Locafi.Client.Contract.Repo
{
    public interface IAuthenticationRepo
    {
        Task<AuthenticationResponseDto> Login (string username, string password);
        Task<AuthenticationResponseDto> RefreshLogin(string refreshToken);
        Task<AuthenticationResponseDto> Login(ILoginCredentialsProvider credentials);
        Task<AuthenticationResponseDto> ReaderLogin(ILoginCredentialsProvider credentials);
        Task<AuthenticationResponseDto> ReaderLogin(string userName, string password);
    }
}