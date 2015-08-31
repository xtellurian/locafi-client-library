using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Data;
using Locafi.Client.Model.Dto.Snapshots;

namespace Locafi.Client.Model.Extensions
{
    public static class SnapshotUri
    {
        /// <summary>
        /// The relative URI for creating/ uploading a new snapshot
        /// </summary>
        /// <param name="snapshotDto">The Snapshot to create/ upload </param>
        /// <returns> The relative URI ie BASE_URL + SERVICE + THIS </returns>
        public static string CreateUri(this AddSnapshotDto snapshotDto)
        {
            return "CreateSnapshot";
        }
    }
}
