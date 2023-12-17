namespace Stas.Monitor.Domains;

using System;
using System.Collections.Generic;

public class DatabaseService : IDatabaseService
{
    private ITemperatureRepository _temperatureRepository;
    private IHumidityRepository _humidityRepository;

    public DatabaseService(ITemperatureRepository temperatureRepository, IHumidityRepository humidityRepository)
    {
        _temperatureRepository = temperatureRepository;
        _humidityRepository = humidityRepository;
    }

    public IEnumerable<Measurement> GetMeasurements(string thermometerName, DateTime fromTime)
    {
        return _temperatureRepository.GetMeasurements(thermometerName, fromTime);
    }

    public IEnumerable<Humidity> GetHumidities(string thermometerName, DateTime fromTime)
    {
        return _humidityRepository.GetHumidities(thermometerName, fromTime);
    }

    public DateTime GetLastMeasurementTimestamp(string thermometerName)
    {
        return _temperatureRepository.GetLastMeasurementTimestamp(thermometerName);
    }
}
