using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Reasons
{
    public class UpdateReasonDto
    {
        public Guid Id { get; set; }

        public string ReasonNumber { get; set; }

        public string Description { get; set; }

        public UpdateReasonDto() { }

        public UpdateReasonDto(ReasonDetailDto dto)
        {
            Id = dto.Id;
            ReasonNumber = dto.ReasonNumber;
            Description = dto.Description;
        }
    }
}
