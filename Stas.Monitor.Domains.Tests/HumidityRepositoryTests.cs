using Microsoft.Data.Sqlite;
namespace Stas.Monitor.Domains.Tests;

[TestFixture]
public class HumidityRepositoryTests
{
    private SqliteConnection _connection;
    private HumidityRepository _repository;

    [SetUp]
    public void Setup()
    {
        // Utiliser SQLite en mémoire avec Cache=Shared
        var connectionString = "DataSource=:memory:;Mode=Memory;Cache=Shared";
        _connection = new SqliteConnection(connectionString);
        _connection.Open();

        CreateDatabaseStructure(_connection);

        var factory = new DbConnectionFactory(connectionString);
        _repository = new HumidityRepository(factory);
    }

    [TearDown]
    public void TearDown()
    {
        _connection.Close();
    }

    private void CreateDatabaseStructure(IDbConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE Humidity (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                thermometer_name TEXT,
                humidity REAL,
                timestamp DATETIME,
                difference REAL
            );";
        command.ExecuteNonQuery();
        command.CommandText = @"
            CREATE TABLE AlertsHumidity (
                type TEXT,
                difference REAL,
                timestamp DATETIME,
                humidity_id REAL,
                FOREIGN KEY(humidity_id) REFERENCES Humidity(id)
            );";
        command.ExecuteNonQuery();
    }

    [Test]
    public void GetHumidities_ShouldReturnsListOfHumidities()
    {
        // Arrange
        var thermometerName = "TestThermometer";
        var fromTime = DateTime.Now.AddHours(-1);

        // Ajouter des données de test
        AddTestHumidityData(_connection, thermometerName, 50.0, DateTime.Now);
        AddTestHumidityData(_connection, thermometerName, 55.0, DateTime.Now);
        // Act
        var result = _repository.GetHumidities(thermometerName, fromTime);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<IEnumerable<Humidity>>(result);
    }

    private void AddTestHumidityData(IDbConnection connection, string thermometerName, double humidity, DateTime timestamp)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Humidity (thermometer_name, humidity, timestamp) VALUES (@ThermometerName, @Humidity, @Timestamp)";
        command.Parameters.Add(new SqliteParameter("@ThermometerName", thermometerName));
        command.Parameters.Add(new SqliteParameter("@Humidity", humidity));
        command.Parameters.Add(new SqliteParameter("@Timestamp", timestamp));
        command.ExecuteNonQuery();
    }
}
