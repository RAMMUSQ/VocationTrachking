using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VocationScheduler;
using VocationScheduler.Jobs.Schedular;

var builder = WebApplication.CreateBuilder(args);

// Hangfire configuration
builder.Services.AddHangfire(config =>
    config.UseMemoryStorage());
builder.Services.AddHangfireServer();

// Diğer bağımlılıkların yapılandırması
builder.Services.AddScoped<IAnnualLeaveService, AnnualLeaveService>();

var app = builder.Build();

app.UseHangfireDashboard();
app.UseHangfireServer();

app.MapGet("/", () => "Vocation Scheduler is running...");

// Hangfire server options
var options = new BackgroundJobServerOptions
{
    WorkerCount = Environment.ProcessorCount * 5
};
app.UseHangfireServer(options);

// Zamanlanmış görevlerin tanımlanması
RecurringJob.AddOrUpdate<Jobs>(job => job.UpdateAnnualLeave(),"1 * * * *");
RecurringJob.AddOrUpdate<BirthdayJob>(job => job.CheckBirthdays(), Cron.Daily); // Her gün çalışacak şekilde ayarlandı

app.Run();