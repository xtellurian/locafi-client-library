using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.RelativeUri
{
    public static class PortalUri
    {
        public static string ServiceName => "Portals";
        public static string GetPortals => "GetPortals";
        public static string CreatePortal => "CreatePortal";
        public static string CreatePortalRule => "CreatePortalRule";

        public static string GetPortal(Guid id)
        {
            return $"GetPortal/{id}";
        }

        public static string GetPortal(string serial)
        {
            return $"GetPortalBySerial/{serial}";
        }

        public static string UpdatePortal(Guid id)
        {
            return $"GetPortal/{id}/UpdatePortal";
        }

        public static string DeletePortal(Guid id)
        {
            return $"DeletePortal/{id}";
        }

        public static string GetPortalRule(Guid id)
        {
            return $"GetPortalRule/{id}";
        }

        public static string GetPortalRules()
        {
            return "GetPortalRules";
        }

        public static string GetPortalRules(Guid id)
        {
            return $"GetPortal/{id}/GetPortalRules";
        }

        public static string UpdatePortalRule(Guid id)
        {
            return $"GetPortalRule/{id}/UpdatePortalRule";        
        }

        public static string DeletePortalRule(Guid id)
        {
            return $"DeletePortalRule/{id}";
        }
    }
}
