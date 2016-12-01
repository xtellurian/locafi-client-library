using System.Collections.Generic;
using Locafi.Client.Model.Enums;

namespace Locafi.Client.Model.Dto.InboundIntegrations
{
    public class FileUploadResultDto
    {
        public FileUploadResultType Result { get; set; }

        public string Description { get; set; }

        public IList<FileUploadResultEntityDto> ResultItems { get; set; }

        public FileUploadResultDto()
        {
            ResultItems = new List<FileUploadResultEntityDto>();
        }


    }
}
