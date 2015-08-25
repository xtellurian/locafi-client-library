using System.Threading.Tasks;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto;

namespace Locafi.Client.Contract.Services
{
    public interface IAuthenticationRepo
    {
        Task<AuthorizationTokenDto> Login (string usrname, string passwrd);
    }
}