using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Configuration;

namespace Locafi.Client.Contract.Repo
{
    public interface IConfigurationRepo
    {
        Task<ConfigurationDto> GetConfigurations();
    }
}