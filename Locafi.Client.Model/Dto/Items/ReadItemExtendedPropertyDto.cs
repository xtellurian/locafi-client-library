using System;
using Locafi.Client.Model.Dto.Skus;

namespace Locafi.Client.Model.Dto.Items
{
    public class ReadItemExtendedPropertyDto : WriteItemExtendedPropertyDto
    {

  //      public Guid? SkuExtendedPropertyId { get; set; }

//        public Guid ExtendedPropertyId { get; set; }

        public string Name { get; set; }

        public bool IsRequired { get; set; }

        public bool IsAllowPrefill { get; set; }

        public string DataType { get; set; }

 //       public string Value { get; set; }

    }
}