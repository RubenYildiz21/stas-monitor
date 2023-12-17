namespace Stas.Monitor.Domains;

public class HumidityRepository : IHumidityRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public HumidityRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IEnumerable<Humidity> GetHumidities(string thermometerName, DateTime fromTime)
    {
        var dataList = new List<Humidity>();
        using var connection = _connectionFactory.CreateConnection();
        using var command = connection.CreateCommand();
        connection.Open();
        command.CommandText = "SELECT h.thermometer_name, h.humidity, h.timestamp, a.type, a.difference, a.humidity_id " +
                              "FROM Humidity h LEFT JOIN AlertsHumidity a ON h.id = a.humidity_id " +
                              "WHERE h.thermometer_name LIKE @ThermometerName AND h.timestamp > @FromTime " +
                              "ORDER BY h.timestamp DESC";

        command.AddParameterWithValue("@ThermometerName", "%" + thermometerName + "%");
        command.AddParameterWithValue("@FromTime", fromTime);

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var data = new Humidity
                {
                    ThermometerName = reader.GetString(reader.GetOrdinal("thermometer_name")),
                    HumidityValue = reader.IsDBNull(reader.GetOrdinal("humidity")) ? 0 : reader.GetDouble(reader.GetOrdinal("humidity")),
                    Timestamp = reader.GetDateTime(reader.GetOrdinal("timestamp")),
                    Difference = reader.IsDBNull(reader.GetOrdinal("difference")) ? 0 : reader.GetDouble(reader.GetOrdinal("difference")),
                };
                dataList.Add(data);
            }
        }

        return dataList;
    }
}
