using System;

namespace LearningKafka.Contracts
{
    public class WeatherData
    {
        public string Location { get; set; }

        public double TemperatureCelsius { get; set; }

        public double Humidity { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}