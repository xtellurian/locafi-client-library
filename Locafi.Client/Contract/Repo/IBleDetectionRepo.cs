using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Ble;

namespace Locafi.Client.Contract.Repo
{
    public interface IBleDetectionRepo
    {
        Task UploadDetections(IEnumerable<BleDetectionBase> detections);
    }
}