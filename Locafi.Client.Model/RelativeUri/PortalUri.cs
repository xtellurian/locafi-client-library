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

        #region -- portals

        public static string GetPortals => @"GetPortals";
        public static string GetPortal(Guid id)
        {
            return $"GetPortal/{id}";
        }
        public static string CreatePortal => @"CreatePortal";
        public static string UpdatePortal => @"UpdatePortal";
        public static string DeletePortal(Guid id)
        {
            return $"DeletePortal/{id}";
        }

        #endregion

        #region -- devices

        public static string CreateDevice => @"CreateDevice";
        public static string UpdateDevice => @"UpdateDevice";
        public static string GetDevices => @"GetDevices";
        public static string GetDevice(Guid id)
        {
            return $"GetDevice/{id}";
        }

        public static string DeleteDevice(Guid id)
        {
            return $"DeleteDevice/{id}";
        }

        #endregion

        #region -- portal rules

        public static string CreatePortalRule => @"CreatePortalRule";
        public static string GetPortalRules => @"GetPortalRules";
        public static string UpdatePortalRule => @"UpdatePortalRule";
        public static string GetPortalRule(Guid id)
        {
            return $"GetPortalRule/{id}";
        }
        public static string DeletePortalRule(Guid id)
        {
            return $"DeletePortalRule/{id}";
        }

        #endregion

        #region -- clusters

        public static string ProcessCluster => @"ProcessCluster";

        #endregion
    }
}
