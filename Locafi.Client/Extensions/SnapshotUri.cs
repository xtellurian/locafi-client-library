using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Data;

namespace Locafi.Client.Model.Extensions
{
    public static class SnapshotUri
    {
        public static string CreateUri(this SnapshotDto snapshotDto)
        {
            return "CreateSnapshot";
        }
    }
}
