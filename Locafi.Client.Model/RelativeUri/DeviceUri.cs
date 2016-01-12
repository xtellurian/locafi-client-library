using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.RelativeUri
{
    public static class DeviceUri
    {
        public static string ServiceName => "Devices";
        public static string GetDevices => "GetDevices";
        public static string CreateDevice => "CreateDevice";
        public static string GetReaders => "GetReaders";
        public static string CreateReader => "CreateReader";


        public static string GetDevice(Guid id)
        {
            return $"GetDevice/{id}";
        }
        
        public static string DeleteDevice(Guid id)
        {
            return $"DeleteDevice/{id}";
        }

        public static string GetReader(Guid id)
        {
            return $"GetReader/{id}";
        }

        public static string GetReader(string serial)
        {
            return $"GetReaderBySerial/{serial}";
        }        

        public static string Delete(Guid id)
        {
            return $"DeleteReader/{id}";
        }
    }
}
