using System.Collections.Generic;

namespace Locafi.Client.Model.Dto.InboundIntegrations
{
    public class FileUploadDto
    {
        public IList<FileUploadEntityDto> Entities { get; set; }

        public string UniqueProperty { get; set; }

        public string FileUploadOperation { get; set; }

        public FileUploadDto()
        {
            Entities = new List<FileUploadEntityDto>();
        }
    }
}
