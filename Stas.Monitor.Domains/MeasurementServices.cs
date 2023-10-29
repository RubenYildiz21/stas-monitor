using System.Globalization;
using Serilog;

namespace Stas.Monitor.Domains;

public class MeasurementServices : IMeasurementServices
{

    private string _csvFilePath;
    
    public MeasurementServices(string csvFilePath)
    {
        this._csvFilePath = csvFilePath;
    }

    public MeasurementServices()
    {
        
    }

    public override IEnumerable<Measurement> GetRecentMeasurements(string thermometerName, DateTime fromTime)
    {
        try
        {
            var lines = File.ReadAllLines(_csvFilePath).Skip(1);
            return lines.Select(line =>
                {
                    var columns = line.Split(',');
                    if (columns.Length < 4)
                    {
                        throw new Exception("Format invalide");
                    }
                    return new Measurement
                    {
                        ThermometerName = columns[0],
                        Timestamp = DateTime.ParseExact(columns[1], "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                        MeasurementType = columns[2],
                        Value = double.Parse(columns[3].Replace("°C", ""), CultureInfo.InvariantCulture),
                    };
                })
                .Where(measurement => measurement.ThermometerName == thermometerName && measurement.Timestamp >= fromTime).OrderByDescending(measurement => measurement.Timestamp);
        }
        catch (FileNotFoundException ex)
        {
            Log.Error($"File not found: {_csvFilePath}", ex);
            throw new Exception($"Erreur lors de la lecture du fichier de mesures csv: {_csvFilePath}", ex);
        }
    }
}