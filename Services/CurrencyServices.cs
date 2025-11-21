using RateMate.Models;
using System.Net.Http.Json;

namespace RateMate.Services
{
    public class CurrencyServices : ICurrencyServices
    {
        private readonly HttpClient _httpClient;
        private const string ApiBaseUrl = "https://api.exchangerate-api.com/v4/latest/";

        public CurrencyServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ExchangeRateResponse?> GetExchangeRatesAsync(string baseCurrency)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ExchangeRateResponse>(
                    $"{ApiBaseUrl}{baseCurrency}"
                );
                return response;
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
