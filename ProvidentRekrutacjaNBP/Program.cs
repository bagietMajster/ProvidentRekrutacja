using NBP.Context;
using NBP.Worker.Common;
using NBP.Worker.Quartz;
using Quartz;
using static NBP.Worker.Quartz.QuartzConfiguration;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHttpClient();

builder.Services.AddSqlite<NbpContext>("Data Source=Nbp.db; Pooling=false");

builder.Services.AddSingleton<IExchangeRateService, ExchangeRateService>();

builder.Services.AddQuartz();
builder.Services.AddQuartzHostedService(opt =>
    opt.WaitForJobsToComplete = true
    );

var host = builder.Build();

await host.Services.AddQuartzJobs();

host.Run();