using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LearningKafka.Producer
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherData>> GetWeatherDataAsync(CancellationToken cancellationToken);
    }
}