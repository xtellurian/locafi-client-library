// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Reasons;
using Locafi.Client.Model.Enums;

namespace Locafi.Client.Contract.Repo
{
    public interface IReasonRepo
    {
        Task<IList<ReasonDetailDto>> GetAllReasons();
        Task<ReasonDetailDto> CreateReason(AddReasonDto reasonDto);
        Task Delete(Guid id);
        Task<IList<ReasonDetailDto>> GetReasonsFor(ReasonFor reasonFor);
    }
}