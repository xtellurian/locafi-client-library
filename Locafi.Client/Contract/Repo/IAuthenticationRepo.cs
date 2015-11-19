// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Authentication;

namespace Locafi.Client.Contract.Repo
{
    public interface IAuthenticationRepo
    {
        Task<AuthenticationResponseDto> Login (string emailAddress, string password);
        Task<AuthenticationResponseDto> RefreshLogin(string refreshToken);
        Task<AuthenticationResponseDto> Login(ILoginCredentialsProvider credentials);
        Task<AuthenticationResponseDto> PortalLogin(ILoginCredentialsProvider credentials);
        Task<AuthenticationResponseDto> PortalLogin(string serial, string password);
    }
}