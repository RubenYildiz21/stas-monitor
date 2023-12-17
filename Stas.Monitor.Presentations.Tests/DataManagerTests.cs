namespace Stas.Monitor.Presentations.Tests;

[TestFixture]
public class DataManagerTests
{
    private Mock<IMeasurementServices> _mockMeasurementService;
    private DataManager _dataManager;
    private Configuration _configuration;
    private ThermometerConfiguration _currentThermometer;

    [SetUp]
    public void Setup()
    {
        _mockMeasurementService = new Mock<IMeasurementServices>();
        _currentThermometer = new ThermometerConfiguration("TestThermometer", "TimestampFormat", "TempFormat", new Dictionary<string, int>());
        _configuration = new Configuration(new List<ThermometerConfiguration> { _currentThermometer });
        _dataManager = new DataManager(_configuration, _mockMeasurementService.Object, _currentThermometer);
    }

    [Test]
    public void GetRecentMeasurements_ShouldCallsMeasurementService()
    {
        // Arrange
        var thermometerName = "TestThermometer";
        var fromTime = DateTime.Now;
        var expectedMeasurements = new List<Measurement> { new Measurement() };
        _mockMeasurementService.Setup(s => s.GetRecentMeasurements(thermometerName, fromTime)).Returns(expectedMeasurements);

        // Act
        var result = _dataManager.GetRecentMeasurements(thermometerName, fromTime);

        // Assert
        _mockMeasurementService.Verify(s => s.GetRecentMeasurements(thermometerName, fromTime), Times.Once);
        Assert.AreEqual(expectedMeasurements, result);
    }

    [Test]
    public void GetRecentHumidities_ShouldCallsMeasurementService()
    {
        // Arrange
        var thermometerName = "TestThermometer";
        var fromTime = DateTime.Now;
        var expectedHumidities = new List<Humidity> { new Humidity() };
        _mockMeasurementService.Setup(s => s.GetRecentHumidities(thermometerName, fromTime)).Returns(expectedHumidities);

        // Act
        var result = _dataManager.GetRecentHumidities(thermometerName, fromTime);

        // Assert
        _mockMeasurementService.Verify(s => s.GetRecentHumidities(thermometerName, fromTime), Times.Once);
        Assert.AreEqual(expectedHumidities, result);
    }

    [Test]
    public void GetLastMeasurementTimestamp_ShouldCallsMeasurementService()
    {
        // Arrange
        var thermometerName = "TestThermometer";
        var expectedTimestamp = DateTime.Now;
        _mockMeasurementService.Setup(s => s.GetLastMeasurementTimestamp(thermometerName)).Returns(expectedTimestamp);

        // Act
        var result = _dataManager.GetLastMeasurementTimestamp(thermometerName);

        // Assert
        _mockMeasurementService.Verify(s => s.GetLastMeasurementTimestamp(thermometerName), Times.Once);
        Assert.AreEqual(expectedTimestamp, result);
    }

    [Test]
    public void SetCurrentThermometer_ShouldUpdatesCurrentThermometer()
    {
        // Arrange
        var newThermometerName = "NewThermometer";
        var newThermometerConfig = new ThermometerConfiguration(newThermometerName, "TimestampFormat", "TempFormat", new Dictionary<string, int>());
        _configuration.Thermometers.Add(newThermometerConfig);

        // Act
        _dataManager.SetCurrentThermometer(newThermometerName);

        // Assert
        Assert.AreEqual(newThermometerConfig, _dataManager.Configuration.Thermometers.FirstOrDefault(t => t.Name == newThermometerName));
    }

    [Test]
    public void GetRecentMeasurements_WhenServiceThrowsException_ShouldThrowsException()
    {
        // Arrange
        var thermometerName = "TestThermometer";
        var fromTime = DateTime.Now;
        _mockMeasurementService.Setup(s => s.GetRecentMeasurements(thermometerName, fromTime)).Throws(new Exception("stas monitor : unable to read data"));

        // Act & Assert
        Assert.Throws<Exception>(() => _dataManager.GetRecentMeasurements(thermometerName, fromTime));
    }

    [Test]
    public void GetRecentHumidities_WhenServiceThrowsException_ShouldThrowsException()
    {
        // Arrange
        var thermometerName = "TestThermometer";
        var fromTime = DateTime.Now;
        _mockMeasurementService.Setup(s => s.GetRecentHumidities(thermometerName, fromTime)).Throws(new Exception("stas monitor : unable to read data"));

        // Act & Assert
        Assert.Throws<Exception>(() => _dataManager.GetRecentHumidities(thermometerName, fromTime));
    }

}
