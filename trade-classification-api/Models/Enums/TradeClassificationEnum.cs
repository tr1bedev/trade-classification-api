using System.Text.Json.Serialization;

namespace trade_classification_api.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TradeClassificationEnum
    {
        HighRisk,
        MediumRisk,
        LowRisk
    }
}