using Microsoft.EntityFrameworkCore;
using NBP.Context;
using NBP.Worker.Common;
using Quartz;

namespace NBP.Worker.Quartz
{
    public class UpdateDatabaseFromStartingDate : IJob
    {
        private readonly NbpContext _nbpContext;
        private readonly IConfiguration _configuration;
        private readonly IExchangeRateService _exchangeRateService;
        private readonly ILogger<UpdateDatabaseFromStartingDate> _logger;

        public UpdateDatabaseFromStartingDate(NbpContext nbpContext,
                                              IConfiguration configuration,
                                              IExchangeRateService exchangeRateService,
                                              ILogger<UpdateDatabaseFromStartingDate> logger)
        {
            _nbpContext = nbpContext;
            _configuration = configuration;
            _exchangeRateService = exchangeRateService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if (await ShouldFetchExchangeRatesAsync())
            {
                string apiUrl = _configuration["NbpBaseUrl:TableAWithStartEndDate"]
                .Replace("{startDate}", _configuration["StartDate"])
                .Replace("{endDate}", DateTime.Now.ToString("yyyy-MM-dd"));

                var exchangeRates = await _exchangeRateService.GetExchangeRatesFromApiAsync(apiUrl);

                if (!exchangeRates.Any())
                {
                    _logger.LogInformation($"No data from API for time range");
                    return;
                }

                var exchangeRateTables = _exchangeRateService.CreateExchangeRateTables(exchangeRates);

                using (var transaction = await _nbpContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await _nbpContext.ExchangeRateTable.AddRangeAsync(exchangeRateTables);
                        await _nbpContext.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message.ToString());
                        await transaction.RollbackAsync();
                    }
                }
            }
        }

        private async Task<bool> ShouldFetchExchangeRatesAsync()
        {
            return await _nbpContext.ExchangeRateTable.CountAsync() == 0;
        }
    }
}
