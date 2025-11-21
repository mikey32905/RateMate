namespace RateMate.Models
{
    public class ExchangeRateResponse
    {
        public string Base { get; set; } = string.Empty;
        public Dictionary<string, decimal> Rates { get; set; } = new();
        public long Time_Last_Updated { get; set; }
    }
}
