namespace Stas.Monitor.Domains;

public class IMeasurementServices
{
    public virtual IEnumerable<Measurement> GetRecentMeasurements(string thermometerName, DateTime fromTime)
    {
        throw new NotImplementedException();
    }
}