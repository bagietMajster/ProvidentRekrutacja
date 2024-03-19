using Microsoft.EntityFrameworkCore;
using NBP.Context;
using NBP.Worker.Common;
using Quartz;

namespace NBP.Worker.Quartz
{
    public class UpdateDatabaseOncePerDay : IJob
    {
        private readonly NbpContext _nbpContext;
        private readonly IConfiguration _configuration;
        private readonly IExchangeRateService _exchangeRateService;
        private readonly ILogger<UpdateDatabaseFromStartingDate> _logger;

        public UpdateDatabaseOncePerDay(NbpContext nbpContext,
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
            string apiUrl = _configuration["NbpBaseUrl:TableAToday"];
            var exchangeRates = await _exchangeRateService.GetExchangeRatesFromApiAsync(apiUrl);

            if (!exchangeRates.Any())
            {
                _logger.LogInformation($"No data from API for today");
                return;
            }

            var existingDates = await _nbpContext.ExchangeRateTable
                .Where(x => exchangeRates.Select(er => er.effectiveDate)
                .Distinct()
                .Contains(x.EffectiveDate))
                .Select(x => x.EffectiveDate)
                .ToListAsync();

            var newExchangeRates = exchangeRates
                .Where(er => !existingDates.Contains(er.effectiveDate))
                .ToList();

            if (newExchangeRates.Any())
            {
                var newExchangeRateTables = _exchangeRateService.CreateExchangeRateTables(newExchangeRates);
                await _nbpContext.ExchangeRateTable.AddRangeAsync(newExchangeRateTables);
                await _nbpContext.SaveChangesAsync();
            }
        }
    }
}