using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using LearningKafka.Infrastructure.Kafka;
using LearningKafka.Infrastructure.Kafka.Serialization;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LearningKafka.Producer
{
    public class ProducerService : BackgroundService
    {
        private readonly IWeatherService weatherService;
        private readonly IMessageSerializer messageSerializer;
        private readonly IOptions<KafkaProducerOptions> kafkaProducerOptions;
        private readonly ProducerConfig producerConfig;
        private readonly ILogger<ProducerService> logger;

        public ProducerService(
            IWeatherService weatherService,
            IMessageSerializer messageSerializer,
            IOptions<KafkaProducerOptions> kafkaProducerOptions,
            ProducerConfig producerConfig,
            ILogger<ProducerService> logger)
        {
            this.weatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
            this.messageSerializer = messageSerializer ?? throw new ArgumentNullException(nameof(messageSerializer));
            this.kafkaProducerOptions = kafkaProducerOptions ?? throw new ArgumentNullException(nameof(kafkaProducerOptions));
            this.producerConfig = producerConfig ?? throw new ArgumentNullException(nameof(producerConfig));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var producer = new ProducerBuilder<string, string>(producerConfig).Build();

            while (!stoppingToken.IsCancellationRequested)
            {
                var weatherData = await weatherService.GetWeatherDataAsync(stoppingToken);

                foreach (var report in weatherData)
                {
                    var message = messageSerializer.Serialize(report);
                    var produceResult = await producer.ProduceAsync(
                        kafkaProducerOptions.Value.Topic,
                        new Message<string, string>
                        {
                            Key = report.Location,
                            Value = message,
                        });

                    logger.LogInformation("Published weather report for location {Location} at {Timestamp}.", report.Location, report.Timestamp);
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }
}