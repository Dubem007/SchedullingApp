using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using SchdullingApp.Jobs;
using SchdullingApp.Scheduler;
using SchdullingApp.Jobfactory;
using SchdullingApp.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IJobFactory, JobFactory>();
builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

#region Adding JobType
builder.Services.AddSingleton<NotificationJob>();
#endregion

#region Adding Jobs 
List<JobSchedule> jobMetadatas = new List<JobSchedule>();
jobMetadatas.Add(new JobSchedule(typeof(NotificationJob), "Notify Job", "0/10 * * * * ?"));

builder.Services.AddSingleton(jobMetadatas);
#endregion

builder.Services.AddHostedService<MyScheduler>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
