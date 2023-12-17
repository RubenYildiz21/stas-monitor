namespace Stas.Monitor.Domains;

public interface IDatabaseService
{
    IEnumerable<Measurement> GetMeasurements(string thermometerName, DateTime fromTime);

    IEnumerable<Humidity> GetHumidities(string thermometerName, DateTime fromTime);

    DateTime GetLastMeasurementTimestamp(string thermometerName);
}
