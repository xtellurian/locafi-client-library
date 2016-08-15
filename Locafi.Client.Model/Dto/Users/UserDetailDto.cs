using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto;
using Locafi.Client.Model.Dto.Users;
using Locafi.Client.Model.Dto.Tags;

namespace Locafi.Client.Model.Dto.Users
{
    public class UserDetailDto : UserSummaryDto
    {
        public UserDetailDto()
        {
            PersonTagList = new List<TagDetailDto>();
            PersonExtendedPropertyList = new List<ReadEntityExtendedPropertyDto>();
        }
        public UserDetailDto(UserDetailDto dto)
        {
            var type = typeof(UserDetailDto);
            var properties = type.GetTypeInfo().DeclaredProperties;
            foreach (var property in properties)
            {
                var value = property.GetValue(dto);
                property.SetValue(this, value);
            }
        }

        public Guid PersonId { get; set; }

        public string ImageUrl { get; set; }

        public IList<ReadEntityExtendedPropertyDto> PersonExtendedPropertyList { get; set; }

        public IList<TagDetailDto> PersonTagList { get; set; }
    }
}
