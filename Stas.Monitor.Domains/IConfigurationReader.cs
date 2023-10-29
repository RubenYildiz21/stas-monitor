namespace Stas.Monitor.Domains;

public interface IConfigurationReader
{
    IEnumerable<Measurement> GetMeasurements();

    Configuration GetConfiguration();
    string GetCsvFilePath();
    string GetCsvAlertFilePath();

}