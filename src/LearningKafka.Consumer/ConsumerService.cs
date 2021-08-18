using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using LearningKafka.Contracts;
using LearningKafka.Infrastructure.Kafka;
using LearningKafka.Infrastructure.Kafka.Serialization;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LearningKafka.Consumer
{
    public class ConsumerService : BackgroundService
    {
        private readonly IOptions<KafkaConsumerOptions> kafkaConsumerOptions;
        private readonly IMessageSerializer messageSerializer;
        private readonly ConsumerConfig consumerConfig;
        private readonly ILogger<ConsumerService> logger;

        public ConsumerService(
            IOptions<KafkaConsumerOptions> kafkaConsumerOptions,
            IMessageSerializer messageSerializer,
            ConsumerConfig consumerConfig,
            ILogger<ConsumerService> logger)
        {
            this.kafkaConsumerOptions = kafkaConsumerOptions ?? throw new ArgumentNullException(nameof(kafkaConsumerOptions));
            this.messageSerializer = messageSerializer ?? throw new ArgumentNullException(nameof(messageSerializer));
            this.consumerConfig = consumerConfig ?? throw new ArgumentNullException(nameof(consumerConfig));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var topic = kafkaConsumerOptions.Value.Topic;
            var groupId = consumerConfig.GroupId;

            using var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();

            logger.LogInformation(
                "Consume loop starting, subscribing to topic {Topic} as subscriber group {GroupId}...",
                topic,
                groupId);

            consumer.Subscribe(topic);

            logger.LogInformation(
                "Successfully subscribed to topic {Topic} as subscriber group {GroupId}.",
                topic,
                groupId);

            while (!stoppingToken.IsCancellationRequested)
            {
                ConsumeResult<string, string> consumeResult;
                try
                {
                    consumeResult = consumer.Consume(stoppingToken);

                    // TODO: There was something about message potentially being null...
                    var message = messageSerializer.Deserialize<WeatherData>(consumeResult.Message.Value);

                    logger.LogInformation(
                        "Temperature at {Location} is {TemperatureC:0.00}°C, humidity is {Humidity:0.00}% at {Timestamp}.",
                        message.Location,
                        message.TemperatureCelsius,
                        message.Humidity,
                        message.Timestamp);
                }
                catch (OperationCanceledException e)
                {
                    logger.LogInformation(e, "Consume loop interrupted, stopping...");
                    break;
                }
            }

            consumer.Close();

            logger.LogInformation("Consume loop terminated.");

            return Task.CompletedTask;
        }
    }
}