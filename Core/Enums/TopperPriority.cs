namespace backend.Core.Enums
{
    /// <summary>
    /// Shows how soon a topper should be used.
    /// </summary>
    public enum TopperPriority
    {
        VerySoon,       //Feed ASAP, it was fed a long time ago or never.
        Soon,           //Feed soon.
        CanWait,        //Was fed somewhat recently, it will be a while until it comes back to the top of the list.
        FurtherNotice,  //Feed pending further notice, meaning not for a while because it was fed very recently.
    }
}
