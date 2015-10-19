using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Enums
{
    public enum ReaderMode
    {
        MaxThroughput = 1,
        Hybrid = 2,
        DenseReaderM4 = 3,
        DenseReaderM8 = 4,
        MaxMiller = 5,
        AutoSetDenseReader = 6,
        AutoSetSingleReader = 7
    }
}
