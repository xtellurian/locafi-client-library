using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.RelativeUri
{
    public static class CycleCountUri
    {
        public static string ServiceName => "CycleCounts";
        public static string GetCycleCounts => "GetFilteredCycleCounts";
        public static string AddCycleCount => "CreateCycleCount";
        public static string ResolveCycleCount => "ResolveCycleCount";

        public static string GetCycleCount(Guid id)
        {
            return $"GetCycleCount/{id}";
        }
    }
}
