namespace backend.Core.Dtos
{
    /// <summary>
    /// Data Transport Object for feed topper, which is a batch PATCH request.
    ///     
    /// TODO: Fix comment
    ///     - FedDate is null if the topper was never fed. 
    ///     - FedDate is not null if it was fed and it's in the system.
    ///     - when user feeds the topper, FedDate is set and topper priority decreases.
    ///     
    /// Both dates are required currently for scalability, for when ToppersController is required to set other dates besides today. ???
    /// </summary>
    public class FeedTopperDto
    {
        public required Guid Id { get; init; }

        public required DateOnly? FedDate { get; set; }
    }
}
