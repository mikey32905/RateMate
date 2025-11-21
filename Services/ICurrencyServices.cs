using RateMate.Models;

namespace RateMate.Services
{
    public interface ICurrencyServices
    {
        List<Currency> GetAvailableCurrencies();
        Task<ExchangeRateResponse?> GetExchangeRatesAsync(string baseCurrency);
    }
}