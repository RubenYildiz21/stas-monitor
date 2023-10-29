using System.Globalization;
using NSubstitute.Exceptions;
using Serilog;
using Stas.Monitor.Domains;

namespace Stas.Monitor.Presentations;

public class DataManager
{
    private Configuration _configuration;
    private MeasurementServices _measurementService;
    private AlertServices _alertServices;
    private ThermometerConfiguration _currentThermometer;
    
    public DataManager(Configuration configuration, MeasurementServices measurementService, AlertServices alertServices, ThermometerConfiguration currentThermometer)
    {
        _configuration = configuration;
        _measurementService = measurementService;
        _alertServices = alertServices;
        _currentThermometer = currentThermometer;
    }
    
    public Configuration Configuration { get => _configuration; }
    
    public IEnumerable<Measurement> GetRecentMeasurements()
    {
        var (lastTimestamp, _) = GetLastTimestampFromCsv(_configuration.MeasurementFilePath, _currentThermometer.Name);
        var fromTime = lastTimestamp.AddMinutes(-1);
        return _measurementService.GetRecentMeasurements(_currentThermometer.Name, fromTime) ?? throw new FormatException("Format lignes mesure invalide");
    }

    public IEnumerable<Alert> GetRecentAlerts(IEnumerable<Measurement> recentMeasurements)
    {
        if (recentMeasurements == null || !recentMeasurements.Any())
        {
            return new List<Alert>();
        }
        DateTime fromTime = recentMeasurements.Min(m => m.Timestamp);
        DateTime toTime = recentMeasurements.Max(m => m.Timestamp);
        return _alertServices.GetRecentAlerts(_currentThermometer.Name, fromTime, toTime);
    }
    private (DateTime timestamp, double temperature) GetLastTimestampFromCsv(string csvFilePath, String thermometerName)
    {
        var lines = File.ReadAllLines(csvFilePath).Reverse();
        var line = lines.FirstOrDefault(l => l.Split(',')[0] == thermometerName && l.Split(',').Length >= 4);
        if (line == null) throw new Exception("Invalid");

        var columns = line.Split(',');
        if (!DateTime.TryParseExact(columns[1], "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var timestamp))
            throw new Exception($"Erreur lors de la lecture du fichier de mesures csv: {csvFilePath}");

        var temperature = double.Parse(columns[3].Replace("°C", ""), CultureInfo.InvariantCulture);
        return (timestamp, temperature);
    }
    
    public void SetCurrentThermometer(string thermometerName)
    {
        _currentThermometer = _configuration.Thermometers.FirstOrDefault(t => t.Name == thermometerName);
        if (_currentThermometer == null)
        {
            Log.Error($"No thermometer found with name: {thermometerName}");
            throw new ArgumentNotFoundException("No thermometer found");
        }
    }
}