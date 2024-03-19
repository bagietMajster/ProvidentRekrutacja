using NBP.Context.Models;
using NBP.Worker.Models;

namespace NBP.Worker.Common
{
    public interface IExchangeRateService
    {
        public Task<List<ExchangeRate>> GetExchangeRatesFromApiAsync(string apiUrl);
        public List<ExchangeRateTable> CreateExchangeRateTables(List<ExchangeRate> exchangeRates);
    }
}
