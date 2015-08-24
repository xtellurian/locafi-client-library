using System.Collections.Generic;
using LegacyNavigatorApi.Models.Dtos;

namespace Locafi.Client.Model.Dto.Places
{
    public class PlaceDetailDto : PlaceSummaryDto
    {
//        public Guid Id { get; set; }

//        public string Name { get; set; }

//        public Guid TemplateId { get; set; }
//        public string TemplateName { get; set; }

        public string Description { get; set; }

        public int TagType { get; set; }
//        public string TagNumber { get; set; }
//        public string TagTypeName { get; set; }

        public long UsageCount { get; set; }

//        public Guid? CreatedByUserId { get; set; }

//        public string CreatedByUserFullName { get; set; }

//        public DateTime DateCreated { get; set; }

//        public Guid? LastModifiedByUserId { get; set; }

//        public string LastModifiedByUserFullName { get; set; }

//        public DateTime? DateLastModified { get; set; }

        public IList<ReadEntityExtendedPropertyDto> PlaceExtendedPropertyList { get; set; }

        public PlaceDetailDto()
        {
                PlaceExtendedPropertyList = new List<ReadEntityExtendedPropertyDto>();
        }

    }
}
