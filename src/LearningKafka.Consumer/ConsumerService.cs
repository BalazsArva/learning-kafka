using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using LearningKafka.Infrastructure.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LearningKafka.Consumer
{
    public class ConsumerService : BackgroundService
    {
        private readonly IOptions<KafkaConsumerOptions> kafkaConsumerOptions;
        private readonly ConsumerConfig consumerConfig;
        private readonly ILogger<ConsumerService> logger;

        public ConsumerService(
            IOptions<KafkaConsumerOptions> kafkaConsumerOptions,
            ConsumerConfig consumerConfig,
            ILogger<ConsumerService> logger)
        {
            this.kafkaConsumerOptions = kafkaConsumerOptions ?? throw new ArgumentNullException(nameof(kafkaConsumerOptions));
            this.consumerConfig = consumerConfig ?? throw new ArgumentNullException(nameof(consumerConfig));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var topic = kafkaConsumerOptions.Value.Topic;
            var groupId = consumerConfig.GroupId;

            using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();

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
                ConsumeResult<Ignore, string> consumeResult;
                try
                {
                    consumeResult = consumer.Consume(stoppingToken);
                }
                catch (OperationCanceledException e)
                {
                    logger.LogInformation(e, "Consume loop interrupted, stopping...");
                    break;
                }

                // TODO: There was something about message potentially being null...
                var message = consumeResult.Message.Value;

                logger.LogInformation("Received message: {Message}", message);
            }

            consumer.Close();

            logger.LogInformation("Consume loop terminated.");

            return Task.CompletedTask;
        }
    }
}