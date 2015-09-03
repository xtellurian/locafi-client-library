using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Reader;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Model.RFID;

namespace Locafi.Client.Model.Extensions
{
    public static class RfidTagConversions
    {
        #region SnapshotTagDto
        public static SnapshotTagDto ConvertToSnapshotTagDto(this IRfidTag tag)
        {
            return new SnapshotTagDto
            {
                TagNumber = tag.TagNumber,
                TagType = tag.TagType
            };
        }

        public static IList<SnapshotTagDto> ConvertToSnapshotTagDtos(this IEnumerable<IRfidTag> tags)
        {
            return tags.Select(tag => tag.ConvertToSnapshotTagDto()).ToList();
        }

        #endregion

        #region ClusterTagDto
        public static ClusterTagDto ConvertToClusterTagDto(this IRfidTag tag)
        {
            return new ClusterTagDto
            {
                TagNumber = tag.TagNumber,
                TagType = tag.TagType
            };
        }

        public static IList<ClusterTagDto> ConvertToClusterTagDtos(this IEnumerable<IRfidTag> tags)
        {
            return tags.Select(tag => tag.ConvertToClusterTagDto()).ToList();
        }

        #endregion
    }
}
