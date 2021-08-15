using System;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LearningKafka.Infrastructure.Kafka
{
    public static class DependencySetup
    {
        private static readonly string ClientId = Guid.NewGuid().ToString("n");

        public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KafkaOptions>(opts => configuration.GetSection(KafkaOptions.SectionName).Bind(opts));

            return services;
        }

        public static IServiceCollection AddKafkaProducer(this IServiceCollection services)
        {
            services.AddSingleton(services =>
            {
                var opts = services.GetRequiredService<IOptions<KafkaOptions>>();

                var producerConfig = new ProducerConfig
                {
                    BootstrapServers = string.Join(",", opts.Value.Servers),
                    ClientId = $"{opts.Value.ClientIdPrefix}-{ClientId}",
                };

                return producerConfig;
            });

            return services;
        }

        public static IServiceCollection AddKafkaConsumer(this IServiceCollection services)
        {
            services.AddSingleton(services =>
            {
                var opts = services.GetRequiredService<IOptions<KafkaOptions>>();

                var consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = string.Join(",", opts.Value.Servers),
                    ClientId = $"{opts.Value.ClientIdPrefix}-{ClientId}",
                    GroupId = opts.Value.GroupId,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                };

                return consumerConfig;
            });

            return services;
        }
    }
}