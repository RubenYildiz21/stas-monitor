namespace Stas.Monitor.Domains;

public interface IHumidityRepository
{
    IEnumerable<Humidity> GetHumidities(string thermometerName, DateTime fromTime);
}
