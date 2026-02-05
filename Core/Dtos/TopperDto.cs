using backend.Core.Enums;

namespace backend.Core.Dtos
{
    /// <summary>
    /// Data Transport Object for Get request.
    /// </summary>
    public class TopperDto
    {
        // Only set once
        public required Guid Id { get; init; }

        // Only set once
        public required string Name { get; init; }

        // Only set once
        public required int Calories { get; init; }

        public required TopperPriority Priority { get; set; }

        public DateOnly? ExpiryDate { get; set; }

        public DateOnly? FedDate { get; set; }
    }
}
