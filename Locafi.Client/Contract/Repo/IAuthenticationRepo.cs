using System.Threading.Tasks;
using Locafi.Client.Model.Dto;

namespace Locafi.Client.Contract.Repo
{
    public interface IAuthenticationRepo
    {
        Task<AuthorizationTokenDto> Login (string usrname, string passwrd);
        Task<AuthorizationTokenDto> RefreshLogin(string refreshToken);
    }
}