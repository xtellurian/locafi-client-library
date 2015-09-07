﻿using System;
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