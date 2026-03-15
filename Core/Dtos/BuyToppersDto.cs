namespace backend.Core.Dtos
{
    /// <summary>
    /// Data Transport Object for buy topper, which is a batch PUT request.
    /// Topper scheduling:
    ///     - when user buys the toppers in the store, PurchaseDate is set. 
    ///     
    /// PurchaseDate is required currently for APP v2, when ToppersController is required to set other dates besides today.
    /// </summary>
    public class BuyToppersDto
    {
        public required Guid Id { get; init; }

        public required DateOnly? PurchaseDate { get; set; }
    }
}
