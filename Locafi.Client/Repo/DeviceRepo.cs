using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.ErrorHandlers;
using Locafi.Client.Contract.Http;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Devices;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Repo
{
    public class DeviceRepo : WebRepo, IWebRepoErrorHandler, IDeviceRepo
    {
        public DeviceRepo(IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser) 
            : base(new SimpleHttpTransferer(),authorisedUnauthorizedConfigService, serialiser, DeviceUri.ServiceName)
        {
        }

        public async Task<IList<PeripheralDeviceSummaryDto>> GetDevices()
        {
            var path = DeviceUri.GetDevices;
            var result = await Get<List<PeripheralDeviceSummaryDto>>(path);
            return result;
        }

        public async Task<PeripheralDeviceDetailDto> GetDevice(Guid id)
        {
            var path = DeviceUri.GetDevice(id);
            var result = await Get<PeripheralDeviceDetailDto>(path);
            return result;
        }

        public async Task<PeripheralDeviceDetailDto> CreateDevice(AddPeripheralDeviceDto addDeviceDto)
        {
            var path = DeviceUri.CreateDevice;
            var result = await Post<PeripheralDeviceDetailDto>(addDeviceDto, path);
            return result;
        }

        public async Task DeleteDevice(Guid id)
        {
            var path = DeviceUri.DeleteDevice(id);
            await Delete(path);
        }

        public async Task<IList<RfidReaderSummaryDto>> GetReaders()
        {
            var path = DeviceUri.GetReaders;
            var result = await Get<List<RfidReaderSummaryDto>>(path);
            return result;
        }

        public async Task<RfidReaderDetailDto> GetReader(Guid id)
        {
            var path = DeviceUri.GetReader(id);
            var result = await Get<RfidReaderDetailDto>(path);
            return result;
        }

        public async Task<RfidReaderDetailDto> GetReader(string serial)
        {
            var path = DeviceUri.GetReader(serial);
            var result = await Get<RfidReaderDetailDto>(path);
            return result;
        }

        public async Task<RfidReaderDetailDto> CreateReader(AddRfidReaderDto addReaderDto)
        {
            var path = DeviceUri.CreateReader;
            var result = await Post<RfidReaderDetailDto>(addReaderDto, path);
            return result;
        }

        public async Task<ClusterResponseDto> ProcessCluster(ClusterDto cluster)
        {
            var path = DeviceUri.ProcessCluster;
            var result = await Post<ClusterResponseDto>(cluster, path);
            return result;
        }

        public async Task DeleteReader(Guid id)
        {
            var path = DeviceUri.Delete(id);
            await Delete(path);
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode)
        {
            throw new DeviceRepoException(serverMessages, statusCode);
        }

        public override async Task Handle(HttpResponseMessage response)
        {
            throw new DeviceRepoException(await response.Content.ReadAsStringAsync());
        }
    }
}
