using System.Globalization;
using Stas.Monitor.Domains;

namespace Stas.Monitor.Infrastructures;

public class CsvMeasurementReader : IConfigurationReader
{
    private string _filePath;
    private IConfigurationReader _configurationReaderImplementation;

    public CsvMeasurementReader(string filePath)
    {
        _filePath = filePath;
        GetMeasurements();
    }

    public IEnumerable<Measurement> GetMeasurements()
    {
        var lines = File.ReadAllLines(_filePath);
        return lines.Select(line =>
        {
            var columns = line.Split(',');
            return new Measurement
            {
                ThermometerName = columns[0],
                Timestamp = DateTime.ParseExact(columns[1], "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                MeasurementType = columns[2],
                Value = double.Parse(columns[3].Replace("°C", ""), CultureInfo.InvariantCulture)
            };
        });
    }

    public Configuration GetConfiguration()
    {
        return _configurationReaderImplementation.GetConfiguration();
    }

    public string GetCsvFilePath()
    {
        return _configurationReaderImplementation.GetCsvFilePath();
    }

    public string GetCsvAlertFilePath()
    {
        return _configurationReaderImplementation.GetCsvAlertFilePath();
    }
}