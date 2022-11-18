using Microsoft.AspNetCore.SignalR;
using Quartz;
using Quartz.Spi;
using SchdullingApp.Model;
using System.Reflection.Metadata;

namespace SchdullingApp.Scheduler
{
    public class MyScheduler : IHostedService
    {
        public IScheduler Scheduler { get; set; }
        private readonly IJobFactory jobFactory;
        private readonly List<JobSchedule> jobSchedule;
        private readonly ISchedulerFactory schedulerFactory;

        public MyScheduler(ISchedulerFactory schedulerFactory, List<JobSchedule> jobSchedule, IJobFactory jobFactory)
        {
            this.jobFactory = jobFactory;
            this.schedulerFactory = schedulerFactory;
            this.jobSchedule = jobSchedule;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //Creating Schdeular
            Scheduler = await schedulerFactory.GetScheduler();
            Scheduler.JobFactory = jobFactory;

            //Suporrt for Multiple Jobs
            jobSchedule?.ForEach(jobSchedule =>
            {
                //Create Job
                IJobDetail jobDetail = CreateJob(jobSchedule);
                //Create trigger
                ITrigger trigger = CreateTrigger(jobSchedule);
                //Schedule Job
                Scheduler.ScheduleJob(jobDetail, trigger, cancellationToken).GetAwaiter();
                //Start The Schedular
            });
            await Scheduler.Start(cancellationToken);
        }

        private ITrigger CreateTrigger(JobSchedule jobMetadata)
        {
            return TriggerBuilder.Create()
                .WithIdentity(jobMetadata.JobType.ToString())
                .WithCronSchedule(jobMetadata.CronExpression)
                .WithDescription(jobMetadata.JobName)
                .Build();
        }

        private IJobDetail CreateJob(JobSchedule jobMetadata)
        {
            return JobBuilder.Create(jobMetadata.JobType)
                .WithIdentity(jobMetadata.JobType.ToString())
                .WithDescription(jobMetadata.JobName)
                .Build();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler.Shutdown();
        }
    }
}
