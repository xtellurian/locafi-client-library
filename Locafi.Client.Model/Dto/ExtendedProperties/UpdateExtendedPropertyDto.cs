using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.ExtendedProperties
{
    public class UpdateExtendedPropertyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public UpdateExtendedPropertyDto() { }

        public UpdateExtendedPropertyDto(ExtendedPropertyDetailDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Description = dto.Description;
        }
    }
}
