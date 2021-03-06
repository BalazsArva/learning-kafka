using System;
using Confluent.Kafka;
using LearningKafka.Infrastructure.Kafka.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LearningKafka.Infrastructure.Kafka
{
    public static class DependencySetup
    {
        // This helps identifying concurrently running clients.
        private static readonly string ClientId = Guid.NewGuid().ToString("n");

        public static IServiceCollection AddKafkaMessageSerialization(this IServiceCollection services)
        {
            return services
                .AddSingleton<IMessageSerializer, JsonMessageSerializer>();
        }

        public static IServiceCollection AddKafkaProducer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KafkaProducerOptions>(opts => configuration.GetSection(KafkaProducerOptions.DefaultSectionName).Bind(opts));

            services.AddSingleton(services =>
            {
                var opts = services.GetRequiredService<IOptions<KafkaProducerOptions>>();

                var producerConfig = new ProducerConfig
                {
                    BootstrapServers = string.Join(",", opts.Value.Servers),
                    ClientId = $"{opts.Value.ClientIdPrefix}-{ClientId}",
                    Partitioner = Partitioner.Consistent,
                };

                return producerConfig;
            });

            return services;
        }

        public static IServiceCollection AddKafkaConsumer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KafkaConsumerOptions>(opts => configuration.GetSection(KafkaConsumerOptions.DefaultSectionName).Bind(opts));

            services.AddSingleton(services =>
            {
                var opts = services.GetRequiredService<IOptions<KafkaConsumerOptions>>();

                var consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = string.Join(",", opts.Value.Servers),
                    ClientId = $"{opts.Value.ClientIdPrefix}-{ClientId}",
                    GroupId = opts.Value.AppId,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                };

                return consumerConfig;
            });

            return services;
        }
    }
}