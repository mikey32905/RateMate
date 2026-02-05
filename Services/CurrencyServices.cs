using RateMate.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace RateMate.Services
{
    public class CurrencyServices : ICurrencyServices
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly bool _useNetlifyFunction;
        private const string ApiBaseUrl = "https://v6.exchangerate-api.com/v6/";

        public CurrencyServices(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;

            // Check if we have a local API key configured
            string? exchangeKey = _config["ExchangeRateAccessKey"];
            _useNetlifyFunction = string.IsNullOrEmpty(exchangeKey);
        }

        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        public async Task<ExchangeRateResponse?> GetExchangeRatesAsync(string baseCurrency, string targetCurrency, double amount)
        {
            try
            {
                if (_useNetlifyFunction)
                {
                    // Use Netlify Function when deployed (no local API key available)
                    var netlifyUrl = $"/.netlify/functions/exchange-rate?from={baseCurrency}&to={targetCurrency}&amount={amount}";

                    var response = await _httpClient.GetFromJsonAsync<ExchangeRateResponse>(
                        netlifyUrl, _jsonOptions
                    );
                    return response;
                }
                else
                {
                    // Use direct API call when running locally with API key in config
                    string? exchangeKey = _config["ExchangeRateAccessKey"];
                    var exchangeUrl = $"{ApiBaseUrl}{exchangeKey}/pair/{baseCurrency}/{targetCurrency}/{amount}";

                    var response = await _httpClient.GetFromJsonAsync<ExchangeRateResponse>(
                        exchangeUrl, _jsonOptions
                    );
                    return response;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching exchange rates: {ex.Message}");
                return null;
            }
        }

        public List<Currency> GetAvailableCurrencies()
        {
            return new List<Currency>
            {
                new Currency { Code = "USD", Name = "US Dollar", Symbol = "$" },
                new Currency { Code = "EUR", Name = "Euro", Symbol = "€" },
                new Currency { Code = "GBP", Name = "British Pound", Symbol = "£" },
                new Currency { Code = "JPY", Name = "Japanese Yen", Symbol = "¥" },
                new Currency { Code = "AUD", Name = "Australian Dollar", Symbol = "A$" },
                new Currency { Code = "CAD", Name = "Canadian Dollar", Symbol = "C$" },
                new Currency { Code = "CHF", Name = "Swiss Franc", Symbol = "Fr" },
                new Currency { Code = "CNY", Name = "Chinese Yuan", Symbol = "¥" },
                new Currency { Code = "INR", Name = "Indian Rupee", Symbol = "₹" },
                new Currency { Code = "MXN", Name = "Mexican Peso", Symbol = "$" }
            };
        }


    }
}
