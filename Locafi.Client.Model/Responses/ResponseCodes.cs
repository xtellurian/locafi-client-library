using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Responses
{
    public enum ResponseCodes
    {
        Unknown = 0,
        AuthenticationFailed = 1,
        DuplicateResource = 2,
        OrganisationNotFound = 3,
        UserNotFound = 4,
        AuthorizationDenied = 5,
        ResourceCreatedSuccessfully = 6,
        ResourceDoesNotExist = 7,
        OperationFailedToComplete = 8,
        ValidationError = 9,
        InvalidForeignKey = 10,
        IllegalBusinessOperation = 11,
        RegistrationFailed = 12,
        InvalidExtendedPropertyData = 13
    }
}
