using System.Collections.Generic;

namespace Locafi.Client.Model.Dto.InboundIntegrations
{
    public class FileUploadEntityDto
    {
        public List<FileUploadEntityPropertyDto> EntityProperties { get; set; }

        public FileUploadEntityDto()
        {
            EntityProperties = new List<FileUploadEntityPropertyDto>();
        }
    }
}
