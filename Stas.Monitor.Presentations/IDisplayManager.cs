using Stas.Monitor.Domains;

namespace Stas.Monitor.Presentations;

public interface IDisplayManager
{
   void UpdateTemperatureDisplay(IEnumerable<Measurement> measurements, bool showTemperature);

   void UpdateHumidityDisplay(IEnumerable<Humidity> humidities, bool showTemperature);
}
