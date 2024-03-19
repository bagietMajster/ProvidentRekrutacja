using NBP.Context.Models;
using NBP.Worker.Models;
using System.Text.Json;

namespace NBP.Worker.Common
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ExchangeRateService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<ExchangeRate>> GetExchangeRatesFromApiAsync(string apiUrl)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<List<ExchangeRate>>(responseBody);

            if (result == null)
            {
                return new List<ExchangeRate>();
            }

            return result;
        }

        public List<ExchangeRateTable> CreateExchangeRateTables(List<ExchangeRate> exchangeRates)
        {
            var createdAtDate = DateTime.Now;
            return exchangeRates.Select(er => new ExchangeRateTable
            {
                CreatedAt = createdAtDate,
                EffectiveDate = er.effectiveDate,
                TableType = er.table,
                ExchangeRates = er.rates.Select(rate => new ExchangeRateValue
                {
                    CurrencyCode = rate.currency,
                    Mid = rate.mid
                }).ToList()
            }).ToList();
        }
    }
}
