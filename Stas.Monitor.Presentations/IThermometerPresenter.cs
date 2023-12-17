using Stas.Monitor.Domains;

namespace Stas.Monitor.Presentations;

public interface IThermometerPresenter
{
    Task UpdateDataItemsTemperature(IEnumerable<Measurement> measurements);

    Task UpdateDataItemsHumidity(IEnumerable<Humidity> measurements);

    void ClearTemperatureData();

    void ClearHumidityData();
}
