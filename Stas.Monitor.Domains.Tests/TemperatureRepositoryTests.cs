namespace Stas.Monitor.Domains.Tests;

[TestFixture]
public class TemperatureRepositoryTests
{
    private IDbConnection _connection;
    private TemperatureRepository _repository;

    [SetUp]
    public void Setup()
    {
        var connectionString = "DataSource=:memory:;Mode=Memory;Cache=Shared";
        _connection = new SqliteConnection(connectionString);
        _connection.Open();

        CreateDatabaseStructure(_connection);

        var factory = new DbConnectionFactory(connectionString);
        _repository = new TemperatureRepository(factory);
    }

    private void CreateDatabaseStructure(IDbConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE Temperature (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                thermometer_name TEXT,
                temperature REAL,
                timestamp DATETIME
            );";
        command.ExecuteNonQuery();
        command.CommandText = @"
            CREATE TABLE AlertsTemperature (
                type TEXT,
                difference REAL,
                timestamp DATETIME,
                temperature_id INTEGER,
                FOREIGN KEY(temperature_id) REFERENCES Temperature(id)
            );";
        command.ExecuteNonQuery();
    }

    [Test]
    public void GetMeasurements_ShouldReturnsListOfMeasurements()
    {
        // Arrange
        var thermometerName = "TestThermometer";
        var fromTime = DateTime.Now.AddHours(-1);
        AddTestTemperatureData(_connection, thermometerName, 25.0, DateTime.Now);

        // Act
        var result = _repository.GetMeasurements(thermometerName, fromTime);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<IEnumerable<Measurement>>(result);
    }

    private void AddTestTemperatureData(IDbConnection connection, string thermometerName, double temperature, DateTime timestamp)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Temperature (thermometer_name, temperature, timestamp) VALUES (@ThermometerName, @Temperature, @Timestamp)";
        command.Parameters.Add(new SqliteParameter("@ThermometerName", thermometerName));
        command.Parameters.Add(new SqliteParameter("@Temperature", temperature));
        command.Parameters.Add(new SqliteParameter("@Timestamp", timestamp));
        command.ExecuteNonQuery();
    }

    [Test]
    public void GetLastMeasurementTimestamp_ShouldReturnsCorrectTimestamp()
    {
        // Arrange
        var thermometerName = "TestThermometer";
        var expectedTimestamp = DateTime.Now;
        AddTestTemperatureData(_connection, thermometerName, 25.0, expectedTimestamp);

        // Act
        var result = _repository.GetLastMeasurementTimestamp(thermometerName);

        // Assert
        Assert.AreEqual(expectedTimestamp, result);
    }

    [Test]
    public void GetLastMeasurementTimestamp_WithNonExistentThermometer_ShouldReturnsDateTimeMinValue()
    {
        // Arrange
        var nonExistentThermometerName = "NonExistentThermometer";

        // Act
        var result = _repository.GetLastMeasurementTimestamp(nonExistentThermometerName);

        // Assert
        Assert.AreEqual(DateTime.MinValue, result);
    }

    [Test]
    public void GetLastMeasurementTimestamp_WithEmptyDatabase_ShouldReturnsDateTimeMinValue()
    {
        // Arrange
        var thermometerName = "TestThermometer";

        // Assurez-vous que la base de données est vide
        ClearTemperatureData(_connection);

        // Act
        var result = _repository.GetLastMeasurementTimestamp(thermometerName);

        // Assert
        Assert.AreEqual(DateTime.MinValue, result);
    }

    private void ClearTemperatureData(IDbConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Temperature";
        command.ExecuteNonQuery();
    }

    [TearDown]
    public void TearDown()
    {
        _connection.Close();
    }
}
