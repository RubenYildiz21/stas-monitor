namespace Stas.Monitor.Domains;

public class TemperatureRepository : ITemperatureRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TemperatureRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IEnumerable<Measurement> GetMeasurements(string thermometerName, DateTime fromTime)
    {
        var dataList = new List<Measurement>();
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();
        const string sqlQuery = "SELECT t.thermometer_name, t.temperature, t.timestamp, a.type, a.difference, a.temperature_id " +
                                "FROM Temperature t LEFT JOIN AlertsTemperature a ON t.id = a.temperature_id " +
                                "WHERE t.thermometer_name LIKE @ThermometerName AND t.timestamp > @FromTime " +
                                "ORDER BY t.timestamp DESC";
        using var command = connection.CreateCommand();
        command.CommandText = sqlQuery;
        command.AddParameterWithValue("@ThermometerName", "%" + thermometerName + "%");
        command.AddParameterWithValue("@FromTime", fromTime);
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var data = new Measurement
                {
                    ThermometerName = reader.GetString(reader.GetOrdinal("thermometer_name")),
                    Temperature = reader.IsDBNull(reader.GetOrdinal("temperature")) ? 0 : reader.GetDouble(reader.GetOrdinal("temperature")),
                    Timestamp = reader.GetDateTime(reader.GetOrdinal("timestamp")),
                    Difference = reader.IsDBNull(reader.GetOrdinal("difference")) ? 0 : reader.GetDouble(reader.GetOrdinal("difference")),
                };
                dataList.Add(data);
            }
        }

        return dataList;
    }

    public DateTime GetLastMeasurementTimestamp(string thermometerName)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();
        const string sqlQuery = "SELECT MAX(timestamp) FROM Temperature WHERE thermometer_name = @ThermometerName";
        using var command = connection.CreateCommand();
        command.CommandText = sqlQuery;
        command.AddParameterWithValue("@ThermometerName", thermometerName);
        var result = command.ExecuteScalar();
        return result != DBNull.Value ? Convert.ToDateTime(result) : DateTime.MinValue;
    }
}
