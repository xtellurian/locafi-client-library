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
using Locafi.Client.Model;
using Locafi.Client.Model.Query;

namespace Locafi.Client.Repo
{
    public class DeviceRepo : WebRepo, IWebRepoErrorHandler, IDeviceRepo
    {
        public DeviceRepo(IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser) 
            : base(new SimpleHttpTransferer(),authorisedConfigService, serialiser, DeviceUri.ServiceName)
        {
        }

        public async Task<PageResult<PeripheralDeviceSummaryDto>> QueryDevices(IRestQuery<PeripheralDeviceSummaryDto> query)
        {
            return await QueryDevices(query.AsRestQuery());
        }

        public async Task<PageResult<PeripheralDeviceSummaryDto>> QueryDevices(string oDataQueryOptions = null)
        {
            var path = DeviceUri.GetDevices;

            // add the query options if required
            if(!string.IsNullOrEmpty(oDataQueryOptions))
            {
                if (oDataQueryOptions[0] != '?')
                    path += "?";

                path += oDataQueryOptions;
            }

            // make sure the query asks to return the item count
            if(!path.Contains("$count"))
            {
                if (path.Contains("?"))
                    path += "&$count=true";
                else
                    path += "?$count=true";
            }

            // run query
            var result = await Get<PageResult<PeripheralDeviceSummaryDto>>(path);
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

        public async Task<PageResult<RfidReaderSummaryDto>> QueryReaders(IRestQuery<RfidReaderSummaryDto> query)
        {
            return await QueryReaders(query.AsRestQuery());
        }

        public async Task<PageResult<RfidReaderSummaryDto>> QueryReaders(string oDataQueryOptions = null)
        {
            var path = DeviceUri.GetReaders;

            // add the query options if required
            if (!string.IsNullOrEmpty(oDataQueryOptions))
            {
                if (oDataQueryOptions[0] != '?')
                    path += "?";

                path += oDataQueryOptions;
            }

            // make sure the query asks to return the item count
            if (!path.Contains("$count"))
            {
                if (path.Contains("?"))
                    path += "&$count=true";
                else
                    path += "?$count=true";
            }

            var result = await Get<PageResult<RfidReaderSummaryDto>>(path);
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

        [Obsolete]
        public async Task<ClusterResponseDto> ProcessCluster(ClusterDto cluster)
        {
            var path = ClusterUri.ProcessCluster;
            var result = await Post<ClusterResponseDto>(cluster, path);
            return result;
        }

        public async Task DeleteReader(Guid id)
        {
            var path = DeviceUri.Delete(id);
            await Delete(path);
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
        {
            throw new DeviceRepoException(serverMessages, statusCode, url, payload);
        }

        public override async Task Handle(HttpResponseMessage response, string url, string payload)
        {
            throw new DeviceRepoException($"{url} -- {payload} -- " + await response.Content.ReadAsStringAsync());
        }
    }
}
