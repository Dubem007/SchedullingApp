namespace SchdullingApp.Model
{
    public class JobSchedule
    {
        public JobSchedule(Type jobType,string jobName, string cronExpression)
        {
            JobType = jobType;
            JobName = jobName;
            CronExpression = cronExpression;

        }

        public Type JobType { get; }
        public string JobName { get; set; }
        public string CronExpression { get; }
    }
}
