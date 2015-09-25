using System;

namespace Locafi.Client.Model.RelativeUri
{
    public static class ReaderUri
    {
        public static string ServiceName => "Reader";
        public static string GetReaders => "GetReaders";
        public static string ProcessCluster => "ProcessCluster";
        public static string Login => "Login";
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
