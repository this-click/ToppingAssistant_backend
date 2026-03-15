namespace backend.Core.Dtos
{
    /// <summary>
    /// Data Transport Object for feed topper.
    ///     
    ///     - FedDate is null if the topper was never fed. 
    ///     - FedDate is not null if it was fed before.
    ///     - When user feeds the topper, FedDate is set and topper priority decreases.
    ///     
    /// FedDate is required currently for APP v2, when ToppersController is required to set other dates besides today.
    /// </summary>
    public class FeedTopperDto
    {
        public required Guid Id { get; init; }

        public required DateOnly? FedDate { get; set; }
    }
}
