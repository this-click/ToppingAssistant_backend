namespace backend.Core.Dtos
{
    /// <summary>
    /// Data Transport Object for Update request.
    /// Topper scheduling:
    ///     - when user buys the toppers in the store, PurchaseDate is set. 
    ///     - FedDate is null if the topper was never fed. 
    ///     - FedDate is not null if it was fed and it's in the system.
    ///     - when user feeds the topper, FedDate is set and topper priority decreases.
    ///     
    /// Both dates are required currently for scalability, for when ToppersController is required to set other dates besides today. 
    /// </summary>
    public class TopperUpdateDto
    {
        public required Guid Id { get; init; }

        public required DateOnly? PurchaseDate { get; set; }

        public required DateOnly? FedDate { get; set; }
    }
}
