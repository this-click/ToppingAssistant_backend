using backend.Core.Enums;

namespace backend.Core.Entities
{
    /// <summary>
    /// Blueprint of Topper objects. 
    /// 
    /// They are super foods to be added to normal meals based on a monthly rotation.
    /// The toppers are selected from Dr. Becker's book, Core Longevity Toppers chapter.
    /// 
    /// </summary>
    public record Topper
    {
        //Mandatory, immutable
        public required Guid Id { get; init; }

        //Mandatory, immutable
        public required string Name { get; init; }

        //Mandatory, immutable. Represents kilo calories per 100g.
        public required int Calories { get; init; }

        //Mandatory, mutable
        public required TopperPriority Priority { get; set; }

        //Optional, mutable
        public DateOnly? PurchaseDate { get; set; }

        //Optional, mutable
        public DateOnly? FedDate { get; set; }
    }
}
