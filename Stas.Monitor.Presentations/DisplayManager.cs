using Stas.Monitor.Domains;

namespace Stas.Monitor.Presentations;

public class DisplayManager : IDisplayManager
{
    private readonly IThermometerPresenter _thermometerPresenter;

    public DisplayManager(IThermometerPresenter thermometerPresenter)
    {
        _thermometerPresenter = thermometerPresenter;
    }

    public void UpdateTemperatureDisplay(IEnumerable<Measurement> measurements, bool showTemperature)
    {
        if (!showTemperature)
        {
            _thermometerPresenter.ClearTemperatureData();
            return;
        }

        _thermometerPresenter.UpdateDataItemsTemperature(measurements);
    }

    public void UpdateHumidityDisplay(IEnumerable<Humidity> humidities, bool showHumidity)
    {
        if (!showHumidity)
        {
            _thermometerPresenter.ClearHumidityData();
            return;
        }

        _thermometerPresenter.UpdateDataItemsHumidity(humidities);
    }
}
