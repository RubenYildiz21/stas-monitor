namespace Stas.Monitor.Presentations;

using Domains;
using Serilog;

public class DataManager : IDataManager
{
    private readonly IMeasurementServices _measurementService;
    private ThermometerConfiguration _currentThermometer;
    private Configuration _configuration;

    public DataManager(Configuration configuration, IMeasurementServices measurementService, ThermometerConfiguration currentThermometer)
    {
        _configuration = configuration;
        _measurementService = measurementService;
        _currentThermometer = currentThermometer;
    }

    public Configuration Configuration { get => _configuration; }

    public IEnumerable<Measurement> GetRecentMeasurements(string thermometerName, DateTime fromTime)
    {
        try
        {
           return _measurementService.GetRecentMeasurements(thermometerName, fromTime);
        }
        catch (Exception e)
        {
            Log.Error("stas monitor : unable to read data");
            throw new Exception($"stas monitor : unable to read data {e.Message}");
        }
    }

    public IEnumerable<Humidity> GetRecentHumidities(string thermometerName, DateTime fromTime)
    {
        try
        {
            return _measurementService.GetRecentHumidities(thermometerName, fromTime);
        }
        catch (Exception e)
        {
            Log.Error("stas monitor : unable to read data ");
            throw new Exception($"stas monitor : unable to read data {e.Message}");
        }
    }

    public DateTime GetLastMeasurementTimestamp(string thermometerName)
    {
        try
        {
            return _measurementService.GetLastMeasurementTimestamp(thermometerName);
        }
        catch (Exception e)
        {
            Log.Error($"stas monitor : Error fetching last measurement timestamp because : {e.Message} ");
            return DateTime.Now;
        }
    }

    public void SetCurrentThermometer(string thermometerName)
    {
        _currentThermometer = _configuration.Thermometers.FirstOrDefault(t => t.Name == thermometerName);
        if (_currentThermometer == null)
        {
            Log.Error($"stas monitor : No thermometer found with name: {thermometerName}");
        }
    }
}
