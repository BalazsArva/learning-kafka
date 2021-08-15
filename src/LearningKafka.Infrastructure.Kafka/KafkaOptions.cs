namespace LearningKafka.Infrastructure.Kafka
{
    public class KafkaOptions
    {
        public const string SectionName = "Kafka";

        public string[] Servers { get; set; }

        public string ClientIdPrefix { get; set; }

        public string GroupId { get; set; }
    }
}