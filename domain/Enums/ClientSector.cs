using System.Text.Json.Serialization;

namespace domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ClientSector
    {
        Unknown,
        Public,
        Private
    }
}