using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LearningKafka.Contracts;

namespace LearningKafka.Producer
{
    public class WeatherService : IWeatherService
    {
        private readonly Random random = new Random();

        public Task<IEnumerable<WeatherData>> GetWeatherDataAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<WeatherData>>(new[]
            {
                GetDummyData("San Francisco"),
                GetDummyData("Miami"),
                GetDummyData("Austin"),
                GetDummyData("Houston"),
                GetDummyData("Tampa"),
                GetDummyData("Orlando"),
                GetDummyData("Albuquerque"),
                GetDummyData("San Antonio"),
                GetDummyData("Nashville"),
                GetDummyData("Los Angeles"),
                GetDummyData("San Diego"),
                GetDummyData("San José"),
                GetDummyData("Sacramento"),
                GetDummyData("Phoenix"),
            });
        }

        private WeatherData GetDummyData(string location)
        {
            return new WeatherData
            {
                Location = location,
                Humidity = random.NextDouble(),
                TemperatureCelsius = 20 + random.NextDouble() * 20,
                Timestamp = DateTimeOffset.Now,
            };
        }
    }
}