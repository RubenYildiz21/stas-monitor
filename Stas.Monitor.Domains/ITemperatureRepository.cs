namespace Stas.Monitor.Domains;

public interface ITemperatureRepository
{
    IEnumerable<Measurement> GetMeasurements(string thermometerName, DateTime fromTime);

    DateTime GetLastMeasurementTimestamp(string thermometerName);
}
