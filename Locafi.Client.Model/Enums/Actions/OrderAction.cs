using System;

namespace Locafi.Client.Model.Enums.Actions
{
    [Flags]
    public enum OrderAction
    {
        None = 0,
        Create = 1,
        Allocate = 2, // ie, when a client is scanning tags
        SubmitAllocate = 4,  // ie when a client uploads a snapshot
        Dispatch = 8,
        Receive = 16,
        SubmitReceive = 32,
        Cancel = 64
    }
}
