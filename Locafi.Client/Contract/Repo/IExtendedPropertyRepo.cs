// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Query;
using Locafi.Client.Model;
using Locafi.Client.Model.Dto.ExtendedProperties;

namespace Locafi.Client.Contract.Repo
{
    public interface IExtendedPropertyRepo
    {
        Task<PageResult<ExtendedPropertySummaryDto>> QueryExtendedProperties(string oDataQueryOptions = null);
        Task<PageResult<ExtendedPropertySummaryDto>> QueryExtendedProperties(IRestQuery<ExtendedPropertySummaryDto> query);
        Task<IQueryResult<ExtendedPropertySummaryDto>> QueryExtendedPropertiesContinuation(IRestQuery<ExtendedPropertySummaryDto> query);
        Task<ExtendedPropertyDetailDto> CreateExtendedProperty(AddExtendedPropertyDto addDto);
        Task<ExtendedPropertyDetailDto> UpdateExtendedProperty(UpdateExtendedPropertyDto updateDto);
        Task<bool> DeleteExtendedProperty(Guid placeId);
        Task<ExtendedPropertyDetailDto> GetExtendedPropertyById(Guid id);
    }
}