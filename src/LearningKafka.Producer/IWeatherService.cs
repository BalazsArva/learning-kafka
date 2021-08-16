using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LearningKafka.Contracts;

namespace LearningKafka.Producer
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherData>> GetWeatherDataAsync(CancellationToken cancellationToken);
    }
}