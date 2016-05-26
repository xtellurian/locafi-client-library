using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Uri
{
    public static class PlaceUri
    {
        public static string ServiceName => "Places";
        public static string GetPlaces => "GetFilteredPlaces";
        public static string CreatePlace => "CreatePlace";

        public static string GetPlace(Guid id)
        {
            return $"GetPlace/{id}";
        }

        public static string DeletePlace(Guid id)
        {
            return $"DeletePlace/{id}";
        }

    }
}
