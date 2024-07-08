using Hangfire;
using Hangfire.MySql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        var builder = new HostBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                var connectionString = context.Configuration.GetConnectionString("MySqlConStr");

                services.AddHangfire(config =>
                {
                    config.UseStorage(new MySqlStorage(connectionString));
                });

                services.AddHangfireServer();

                // RecurringJobService'i servis olarak ekleyin
                services.AddHostedService<RecurringJobService>();

                // Buraya diğer servisleri de ekleyebilirsiniz
                // services.AddScoped<IMyService, MyService>();

            }).UseConsoleLifetime();

        var host = builder.Build();

        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;
            try
            {
                var backgroundJobClient = services.GetRequiredService<IBackgroundJobClient>();
                backgroundJobClient.Enqueue(() => Console.WriteLine("Hangfire is configured and running!"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        host.Run();
    }
}