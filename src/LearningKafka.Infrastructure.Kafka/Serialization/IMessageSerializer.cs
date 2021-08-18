namespace LearningKafka.Infrastructure.Kafka.Serialization
{
    public interface IMessageSerializer
    {
        string Serialize<TMessage>(TMessage message);

        TMessage Deserialize<TMessage>(string serializedMessage);
    }
}