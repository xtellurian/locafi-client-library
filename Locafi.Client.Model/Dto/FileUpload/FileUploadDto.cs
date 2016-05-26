using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Locafi.Client.Model.Dto.FileUpload
{
    public class FileUploadDto
    {
        public List<Dictionary<string, string>> Entities { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public FileUploadOperation Operation { get; set; }

        public string UniqueProperty { get; set; }

        public string FileName { get; set; }
    }
}
