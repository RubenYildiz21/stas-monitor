using MySql.Data.MySqlClient;
using Serilog;

namespace Stas.Monitor.Domains;

public class MeasurementServices : IMeasurementServices
{
    private readonly IDatabaseService _databaseService;

    public MeasurementServices(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public IEnumerable<Measurement> GetRecentMeasurements(string thermometerName, DateTime fromTime)
    {
        try
        {
            return _databaseService.GetMeasurements(thermometerName, fromTime);
        }
        catch (MySqlException ex)
        {
            Log.Error("stas monitor : Unable to connect to database");
            throw new Exception($"stas monitor : Unable to connect to database {ex.Message}");
        }
    }

    public IEnumerable<Humidity> GetRecentHumidities(string thermometerName, DateTime fromTime)
    {
        try
        {
            return _databaseService.GetHumidities(thermometerName, fromTime);
        }
        catch (MySqlException ex)
        {
            Log.Error("stas monitor : Unable to connect to database");
            throw new Exception($"stas monitor :Unable to connect to database {ex.Message}");
        }
    }

    public DateTime GetLastMeasurementTimestamp(string thermometerName)
    {
        try
        {
            return _databaseService.GetLastMeasurementTimestamp(thermometerName);
        }
        catch (MySqlException e)
        {
            Log.Error("stas monitor : Unable to connect to database");
            throw new Exception($"stas monitor :Unable to connect to database {e.Message}");
        }
    }
}
