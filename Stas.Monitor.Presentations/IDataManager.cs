using Stas.Monitor.Domains;

namespace Stas.Monitor.Presentations;

public interface IDataManager
{
    Configuration Configuration { get; }

    IEnumerable<Measurement> GetRecentMeasurements(string thermometerName, DateTime fromTime);

    IEnumerable<Humidity> GetRecentHumidities(string thermometerName, DateTime fromTime);

    DateTime GetLastMeasurementTimestamp(string thermometerName);

    void SetCurrentThermometer(string thermometerName);
}
