using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Enums
{
    public enum FileIntegrationPreferenceType
    {
        ItemCreate,
        ItemUpdate,
        ItemDelete,
        ItemMovement,
        PersonCreate,
        PersonUpdate,
        PersonDelete,
        PlaceCreate,
        PlaceUpdate,
        PlaceDelete,
    }
}
