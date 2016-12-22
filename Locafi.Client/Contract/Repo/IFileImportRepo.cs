using System.Threading.Tasks;
using Locafi.Client.Model.Dto.InboundIntegrations;

namespace Locafi.Client.Contract.Repo
{
    public interface IFileImportRepo
    {
        Task<FileUploadResultDto> ImportItems(FileUploadDto uploadDto);
        Task<FileUploadResultDto> ImportPlaces(FileUploadDto uploadDto);
        Task<FileUploadResultDto> ImportPersons(FileUploadDto uploadDto);
    }
}