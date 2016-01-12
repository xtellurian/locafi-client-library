using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Devices;

namespace Locafi.Client.Contract.Repo
{
    public interface IDeviceRepo
    {
        Task<IList<PeripheralDeviceSummaryDto>> GetDevices();
        Task<PeripheralDeviceDetailDto> GetDevice(Guid id);
        Task<PeripheralDeviceDetailDto> CreateDevice(AddPeripheralDeviceDto addDeviceDto);
        Task DeleteDevice(Guid id);
        Task<IList<RfidReaderSummaryDto>> GetReaders();
        Task<RfidReaderDetailDto> GetReader(Guid id);
        Task<RfidReaderDetailDto> GetReader(string serial);
        Task<RfidReaderDetailDto> CreateReader(AddRfidReaderDto addReaderDto);
        [Obsolete]
        Task<ClusterResponseDto> ProcessCluster(ClusterDto cluster);
        Task DeleteReader(Guid id);
    }
}