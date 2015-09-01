using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Persons;

namespace Locafi.Client.Contract.Repo
{
    public interface IPersonRepo
    {
        Task<IList<PersonSummaryDto>> GetAllPersons();
        Task<PersonDetailDto> GetPersonById(Guid id);
        Task<PersonDetailDto> CreatePerson(AddPersonDto addPerson);
        Task DeletePerson(Guid id);
    }
}