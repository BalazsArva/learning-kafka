using Newtonsoft.Json;

namespace LearningKafka.Infrastructure.Kafka.Serialization
{
    public class JsonMessageSerializer : IMessageSerializer
    {
        public TMessage Deserialize<TMessage>(string serializedMessage)
        {
            return JsonConvert.DeserializeObject<TMessage>(serializedMessage);
        }

        public string Serialize<TMessage>(TMessage message)
        {
            return JsonConvert.SerializeObject(message);
        }
    }
}