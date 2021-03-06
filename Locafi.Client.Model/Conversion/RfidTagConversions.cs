﻿using System;
using System.Collections.Generic;
using System.Linq;
using Locafi.Client.Model.Dto.Portals.Clusters;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Model.Enums;
using Locafi.Client.Model.RFID;

namespace Locafi.Client.Model.Conversion
{
    public static class RfidTagConversions
    {
        #region SnapshotTagDto
        public static SnapshotTagDto ConvertToSnapshotTagDto(this IRfidTag tag)
        {
            return new SnapshotTagDto
            {
                TagNumber = tag.TagNumber,
                TagType = TagType.PassiveRfid,
                ReadCount = tag.ReadCount,
                Rssi = tag.Rssi,
                LastReadTime = tag.LastReadTime
            };
        }

        public static IList<SnapshotTagDto> ConvertToSnapshotTagDtos(this IEnumerable<IRfidTag> tags)
        {
            return tags.Select(tag => tag.ConvertToSnapshotTagDto()).ToList();
        }

        #endregion

        #region ClusterTagDto
        public static ClusterTagDto ConvertToClusterTagDto(this IRfidTag tag, TagType tagType = TagType.PassiveRfid, DateTime? readTime = null)
        {
            return new ClusterTagDto
            {
                TagNumber = tag.TagNumber,
                TagType = tagType.ToString(),
                AverageRssi = tag.Rssi,
                ReadCount = tag.ReadCount,
                ReadTime = readTime??DateTime.Now
            };
        }

        public static IList<ClusterTagDto> ConvertToClusterTagDtos(this IEnumerable<IRfidTag> tags)
        {
            return tags.Select(tag => tag.ConvertToClusterTagDto()).ToList();
        }

        #endregion
    }
}
