using System.ComponentModel.DataAnnotations;
using domain.Enums;

namespace domain
{
    public record Trade
    {
        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Value must be non-negative.")]
        public decimal Value { get; init; }
        [Required]
        public ClientSector ClientSector { get; init; } = ClientSector.Unknown;
        [Required]
        public string ClientId { get; init; } = String.Empty;
    }
}
