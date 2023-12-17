using MySql.Data.MySqlClient;

namespace Stas.Monitor.Domains.Tests;

[TestFixture]
public class MeasurementServicesTests
{
    private Mock<IDatabaseService> _mockDatabaseService;
    private MeasurementServices _measurementServices;

    [SetUp]
    public void Setup()
    {
        _mockDatabaseService = new Mock<IDatabaseService>();
        _measurementServices = new MeasurementServices(_mockDatabaseService.Object);
    }

    [Test]
    public void GetRecentMeasurements_ShouldCallsDatabaseService()
    {
        // Arrange
        var thermometerName = "TestThermometer";
        var fromTime = DateTime.Now;
        var measurements = new List<Measurement>();
        _mockDatabaseService.Setup(s => s.GetMeasurements(thermometerName, fromTime)).Returns(measurements);

        // Act
        var result = _measurementServices.GetRecentMeasurements(thermometerName, fromTime);

        // Assert
        Assert.AreEqual(measurements, result);
        _mockDatabaseService.Verify(s => s.GetMeasurements(thermometerName, fromTime), Times.Once);
    }

    [Test]
    public void GetRecentHumidities_ShouldCallsDatabaseService()
    {
        // Arrange
        var thermometerName = "TestThermometer";
        var fromTime = DateTime.Now;
        var humidities = new List<Humidity>();
        _mockDatabaseService.Setup(s => s.GetHumidities(thermometerName, fromTime)).Returns(humidities);

        // Act
        var result = _measurementServices.GetRecentHumidities(thermometerName, fromTime);

        // Assert
        Assert.AreEqual(humidities, result);
        _mockDatabaseService.Verify(s => s.GetHumidities(thermometerName, fromTime), Times.Once);
    }

    [Test]
    public void GetLastMeasurementTimestamp_ShouldCallsDatabaseService()
    {
        // Arrange
        var thermometerName = "TestThermometer";
        var timestamp = DateTime.Now;
        _mockDatabaseService.Setup(s => s.GetLastMeasurementTimestamp(thermometerName)).Returns(timestamp);

        // Act
        var result = _measurementServices.GetLastMeasurementTimestamp(thermometerName);

        // Assert
        Assert.AreEqual(timestamp, result);
        _mockDatabaseService.Verify(s => s.GetLastMeasurementTimestamp(thermometerName), Times.Once);
    }

    [Test]
    public void GetRecentMeasurements_ShouldThrowsException()
    {
        // Arrange
        var thermometerName = "TestThermometer";
        var fromTime = DateTime.Now;
        _mockDatabaseService.Setup(s => s.GetMeasurements(thermometerName, fromTime)).Throws(new Exception());

        // Act & Assert
        Assert.Throws<Exception>(() => _measurementServices.GetRecentMeasurements(thermometerName, fromTime));
    }
}
