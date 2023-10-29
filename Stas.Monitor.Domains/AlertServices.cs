using System.Globalization;
using Serilog;

namespace Stas.Monitor.Domains;

public class AlertServices : IAlertServices
{
    public string CsvFilePath { get; }
    public AlertServices(string csvFilePath)
    {
        this.CsvFilePath = csvFilePath;
    }

    public AlertServices()
    {
        
    }
    public override IEnumerable<Alert> GetRecentAlerts(string thermometerName, DateTime fromTime, DateTime toTime)
    {
        try
        {
            var lines = File.ReadAllLines(CsvFilePath).Skip(1);
            return lines.Select(line =>
                {
                    var columns = line.Split(',');
                    if (columns.Length < 4)
                    {
                        return null;
                    }
                    return new Alert
                    {
                        ThermometerName = columns[0],
                        Timestamp = DateTime.ParseExact(columns[1], "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                        ExpectedTemperature = double.Parse(columns[2].Replace("°C", ""), CultureInfo.InvariantCulture),
                        ActualTemperature = double.Parse(columns[3].Replace("°C", ""), CultureInfo.InvariantCulture)
                    };
                })
                .Where(alert => alert != null && alert.ThermometerName == thermometerName && alert.Timestamp >= fromTime && alert.Timestamp <= toTime).OrderByDescending(alert => alert.Timestamp);
            
            
        }
        catch (FileNotFoundException ex)
        {
            Log.Error($"File not found: {CsvFilePath}", ex);
            throw new Exception($"Erreur lors de la lecture du fichier d'alertes csv: {CsvFilePath}", ex);
        }
    }
}