using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Ble
{
    public interface IBleDetection
    {
        string TagNumber { get; }
        DateTime Timestamp { get; }
    }
}
