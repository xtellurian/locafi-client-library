using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.RelativeUri
{
    public static class PortalUri
    {
        public static string ServiceName => @"Portals";
        public static string GetPortals => @"GetFilteredPortals";
        public static string CreatePortal => @"CreatePortal";
        public static string GetPortalRules => @"GetFilteredPortalRules";
        public static string CreatePortalRule => @"CreatePortalRule";
        public static string UpdatePortalStatus => @"UpdatePortalStatus";
        public static string UpdatePortalHeartbeat => @"SendHeartbeat";
        public static string UpdatePortalRule => @"UpdatePortalRule";
        public static string UpdatePortal => @"UpdatePortal";
        public static string CheckAccess => @"CheckAccess";

        public static string GetPortal(Guid id)
        {
            return $"GetPortal/{id}";
        }

        public static string GetPortal(string serial)
        {
            return $"GetPortalBySerial/{serial}";
        }

        public static string DeletePortal(Guid id)
        {
            return $"DeletePortal/{id}";
        }

        public static string GetPortalRule(Guid id)
        {
            return $"GetPortalRule/{id}";
        }

        public static string GetRulesForPortal(Guid id)
        {
            return $"GetRulesForPortal/{id}";
        }

        public static string DeletePortalRule(Guid id)
        {
            return $"DeletePortalRule/{id}";
        }

        public static string GetPortalStatus(Guid id)
        {
            return $"GetPortalStatus/{id}";
        }
    }
}
