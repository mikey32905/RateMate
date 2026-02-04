using RateMate.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace RateMate.Services
{
    public class CurrencyServices : ICurrencyServices
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;    
        private const string ApiBaseUrl = "https://v6.exchangerate-api.com/v6/";

        public CurrencyServices(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;

            string? exchangeKey = _config["ExchangeRateAccessKey"];

            if (!string.IsNullOrEmpty(exchangeKey))
            {
                _httpClient.BaseAddress = new Uri($"{ApiBaseUrl}{exchangeKey}/");
            }
            else
            {
                //deployed to Netlify
                _httpClient.BaseAddress = new Uri(_httpClient.BaseAddress + $"/{ApiBaseUrl}");

            }


        }

        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };


        public async Task<ExchangeRateResponse?> GetExchangeRatesAsync(string baseCurrency, string targetCurrency, double amount)
        {

            string? configKey = "ExchangeRateAccessKey";

            string? devApiKey = _config[configKey]; 
            string? prodApiKey = Environment.GetEnvironmentVariable(configKey);

            var exchangeKey = string.IsNullOrEmpty(devApiKey) ? prodApiKey : devApiKey;


            if (!string.IsNullOrEmpty(configKey)) 
            {
                var exchangeUrl = $"pair/{baseCurrency}/{targetCurrency}/{amount}";

                var response = await _httpClient.GetFromJsonAsync<ExchangeRateResponse>(
                   exchangeUrl, _jsonOptions
                );
                return response;
            }
            else
            {
                Console.WriteLine($"No ExchangeRate-API key found in configuration. Please set the '{configKey}' environment variable or app setting.");
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
