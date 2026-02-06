namespace WeatherApplication.Dtos.Outgoing
{
    public class WeatherForecastResponse
    {
        public DateOnly? Date { get; set; }
        public double? TempMax { get; set; }
        public double? TempMin { get; set; }
        public double? Temp { get; set; }
        public double? FeelLikeMax { get; set; }
        public double? FeelLikeMin { get;set; }
        public double? FeelLike { get; set; }
    }
}
