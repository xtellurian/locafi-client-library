using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Snapshots
{
    public class CreateSnapshotResultDto
    {
        public Guid SnapshotId { get; set; }

        public IList<ItemNotificationDto> UnResolvedItems { get; set; }

    }
}
