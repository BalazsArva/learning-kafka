namespace LearningKafka.Infrastructure.Kafka
{
    public class KafkaConsumerOptions
    {
        public const string DefaultSectionName = "Kafka:Consumer";

        public string AppId { get; set; }

        public string[] Servers { get; set; }

        public string ClientIdPrefix { get; set; }

        public string Topic { get; set; }
    }
}