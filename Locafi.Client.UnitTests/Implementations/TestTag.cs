using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;
using Locafi.Client.Model.RFID;

namespace Locafi.Client.UnitTests.Implementations
{
    public class TestTag : IRfidTag
    {
        public TestTag(string tagNumber)
        {
            TagNumber = tagNumber;
            TagType = TagType.PassiveRfid;
        }

        public string TagNumber { get; set; }
        public TagType TagType { get; set; }
    }
}
