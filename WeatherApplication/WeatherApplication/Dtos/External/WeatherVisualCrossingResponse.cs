using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WeatherApplication.Dtos.External
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public sealed class WeatherVisualCrossingResponse
    {
        public string? ResolvedAddress { get; set; } = "";
        public List<VisualCrossingDay>? Days { get; set; } = [];
       
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public sealed class VisualCrossingDay
    {
        public DateOnly? Datetime { get; set; } = null;
        public double? Tempmax { get; set; } = double.NaN;
        public double? Tempmin { get; set; } = double.NaN;
        public double? Temp { get; set; } = double.NaN;
        public double? FeelLikeMax { get; set; } = double.NaN;
        public double? FeelLikeMin { get; set; } = double.NaN;
        public double? FeelLike { get; set; } = double.NaN;
    }
}
