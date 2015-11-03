namespace Locafi.Client.Model.Enums.Actions
{
    public enum OrderAction
    {
        Create,
        Allocate, // ie, when a client is scanning tags
        SubmitAllocate,  // ie when a client uploads a snapshot
        Dispatch,
        Receive,
        SubmitReceive,
        Cancel
    }
}
