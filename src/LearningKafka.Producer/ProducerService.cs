using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using LearningKafka.Infrastructure.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LearningKafka.Producer
{
    public class ProducerService : BackgroundService
    {
        private readonly IOptions<KafkaProducerOptions> kafkaProducerOptions;
        private readonly ProducerConfig producerConfig;
        private readonly ILogger<ProducerService> logger;

        public ProducerService(
            IOptions<KafkaProducerOptions> kafkaProducerOptions,
            ProducerConfig producerConfig,
            ILogger<ProducerService> logger)
        {
            this.kafkaProducerOptions = kafkaProducerOptions ?? throw new ArgumentNullException(nameof(kafkaProducerOptions));
            this.producerConfig = producerConfig ?? throw new ArgumentNullException(nameof(producerConfig));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();

            while (!stoppingToken.IsCancellationRequested)
            {
                var produceResult = await producer.ProduceAsync(
                    kafkaProducerOptions.Value.Topic,
                    new Message<Null, string>
                    {
                        Value = $"Producer running at: {DateTimeOffset.Now}",
                    });

                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10, stoppingToken);
            }
        }
    }
}