// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0


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
        Task<ClusterResponseDto> ProcessCluster(ClusterDto cluster);
        Task<ReaderDetailDto> GetReaderBySerial(string serial);
    }
}