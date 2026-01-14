using System.Text.Json.Serialization;

namespace domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RiskCategory
    {
        HIGHRISK,
        MEDIUMRISK,
        LOWRISK
    }
}