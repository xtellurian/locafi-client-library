using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Authentication;
using Locafi.Client.Model.Dto.Reader;

namespace Locafi.Client.Contract.Repo
{
    public interface IReaderRepo
    {
        Task<IList<ReaderSummaryDto>> GetReaders();
        Task<ReaderDetailDto> GetReaderById(Guid id);
        Task DeleteReader(Guid id);
        Task<ClusterReponseDto> ProcessCluster(ClusterDto cluster);
        Task<AuthenticationResponseDto> ReaderLogin(ILoginCredentialsProvider credentials);
        Task<AuthenticationResponseDto> ReaderLogin(string userName, string password);
    }
}