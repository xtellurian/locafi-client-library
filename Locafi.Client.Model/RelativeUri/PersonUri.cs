using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Uri
{
    public static class PersonUri
    {
        public static string ServiceName => "Persons";
        public static string GetPersons => "GetFilteredPersons";
        
        public static string CreatePerson => "CreatePerson";

        public static string GetPerson(Guid id)
        {
            return $"GetPerson/{id}";
        }

        public static string DeletePerson(Guid id)
        {
            return $"DeletePerson/{id}";
        }
    }
}
