using System.ComponentModel.DataAnnotations;
using trade_classification_api.Models.Enums;

namespace trade_classification_api.Models
{
    public record Trade
    {
        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Value must be non-negative.")]
        public decimal Value { get; init; }
        [Required]
        public ClientSectorEnum ClientSector { get; init; } = ClientSectorEnum.Unknown;
    }
}
