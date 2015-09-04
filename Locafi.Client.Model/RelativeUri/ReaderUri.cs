using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Uri
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

        public static string Delete(Guid id)
        {
            return $"DeleteReader/{id}";
        }
    }
}
