using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Locafi.Entity.Dto
{
    public class ExtendedProperty
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public string Label { get; set; }
        public bool Required { get; set; }
        public string DataType { get; set; }
        public string Value { get; set; }
    }
}
