using LearningKafka.Infrastructure.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LearningKafka.Producer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddKafkaMessageSerialization();
                    services.AddKafkaProducer(hostContext.Configuration);
                    services.AddSingleton<IWeatherService, WeatherService>();
                    services.AddHostedService<ProducerService>();
                });
    }
}