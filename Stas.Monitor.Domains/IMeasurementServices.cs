namespace Stas.Monitor.Domains;

public interface IMeasurementServices
{
    public IEnumerable<Measurement> GetRecentMeasurements(string thermometerName, DateTime fromTime);

    public IEnumerable<Humidity> GetRecentHumidities(string thermometerName, DateTime fromTime);

    public DateTime GetLastMeasurementTimestamp(string thermometerName);
}
