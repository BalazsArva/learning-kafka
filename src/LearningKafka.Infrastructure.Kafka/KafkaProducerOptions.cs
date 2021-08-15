namespace LearningKafka.Infrastructure.Kafka
{
    public class KafkaProducerOptions
    {
        public const string DefaultSectionName = "Kafka:Producer";

        public string[] Servers { get; set; }

        public string ClientIdPrefix { get; set; }

        public string Topic { get; set; }
    }
}