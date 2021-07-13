using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace ParkingMaster.Models.Rules.Type
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RuleType
    {
        PLATE,
        TIME,
        MOVE
    }
}
