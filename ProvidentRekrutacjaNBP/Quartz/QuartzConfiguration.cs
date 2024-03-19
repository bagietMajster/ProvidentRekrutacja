using Quartz;

namespace NBP.Worker.Quartz
{
    public static class QuartzConfiguration
    {
        public static async Task<IServiceProvider> AddQuartzJobs(this IServiceProvider services)
        {
            var schedulerFactory = services.GetRequiredService<ISchedulerFactory>();
            var scheduler = await schedulerFactory.GetScheduler();

            scheduler.AddUpdateDatabaseFromStartingDateWorker();
            scheduler.AddUpdateDatabaseOncePerDayWorker();

            return services;
        }

        private static async void AddUpdateDatabaseFromStartingDateWorker(this IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<UpdateDatabaseFromStartingDate>()
                .WithIdentity("sample-job")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .StartNow()
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }

        private static async void AddUpdateDatabaseOncePerDayWorker(this IScheduler scheduler)
        {
            var trigger = TriggerBuilder.Create()
                .WithIdentity("DailyTrigger", "DailyGroup")
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(22, 0))
                .Build();

            var job = JobBuilder.Create<UpdateDatabaseOncePerDay>()
                .WithIdentity("YourJob", "YourGroup")
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
