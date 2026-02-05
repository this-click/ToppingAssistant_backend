namespace backend.Core.Dtos
{
    /// <summary>
    /// Data Transport Object for Update request.
    /// Both dates are required in order to support topper scheduling: 
    ///     - when user buys the toppers in the store, ExpiryDate is set. FedDate is null if the topper was never fed. FedDate
    ///         is not null if it was fed and it's in the system.
    ///     - when user feeds the topper, FedDate is set and topper priority decreases.
    /// </summary>
    public class TopperUpdateDto
    {
        public required Guid Id { get; init; }

        public required DateOnly? ExpiryDate { get; set; }

        public required DateOnly? FedDate { get; set; }
    }
}
