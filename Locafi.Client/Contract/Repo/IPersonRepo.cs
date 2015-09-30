// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Persons;
using Locafi.Client.Model.Query;

namespace Locafi.Client.Contract.Repo
{
    public interface IPersonRepo
    {
        Task<IList<PersonSummaryDto>> GetAllPersons();
        Task<PersonDetailDto> GetPersonById(Guid id);
        Task<PersonDetailDto> CreatePerson(AddPersonDto addPerson);
        Task DeletePerson(Guid id);
        Task<IQueryResult<PersonSummaryDto>> QueryPersonsAsync(IRestQuery<PersonSummaryDto> query);
    }
}