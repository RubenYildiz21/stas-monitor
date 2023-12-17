namespace Stas.Monitor.Presentations.Tests;

[TestFixture]
public class DisplayManagerTests
{
    private Mock<IThermometerPresenter> _mockThermometerPresenter;
    private DisplayManager _displayManager;

    [SetUp]
    public void Setup()
    {
        _mockThermometerPresenter = new Mock<IThermometerPresenter>();
        _displayManager = new DisplayManager(_mockThermometerPresenter.Object);
    }

    [Test]
    public void UpdateTemperatureDisplay_WhenShowTemperatureIsTrue_ThenUpdateDataItemsTemperature()
    {
        // Arrange
        var measurements = new List<Measurement>
        {
            new Measurement { ThermometerName = "Thermometer1", Temperature = 25.5, Timestamp = DateTime.Now }
        };

        // Act
        _displayManager.UpdateTemperatureDisplay(measurements, true);

        // Assert
        _mockThermometerPresenter.Verify(p => p.UpdateDataItemsTemperature(measurements), Times.Once);
        _mockThermometerPresenter.Verify(p => p.ClearTemperatureData(), Times.Never);
    }

    [Test]
    public void UpdateTemperatureDisplay_WhenShowTemperatureIsFalse_ThenClearTemperatureData()
    {
        // Act
        _displayManager.UpdateTemperatureDisplay(null, false);

        // Assert
        _mockThermometerPresenter.Verify(p => p.ClearTemperatureData(), Times.Once);
        _mockThermometerPresenter.Verify(p => p.UpdateDataItemsTemperature(It.IsAny<IEnumerable<Measurement>>()), Times.Never);
    }

    [Test]
    public void UpdateHumidityDisplay_WhenShowHumidityIsTrue_ThenUpdateDataItemsHumidity()
    {
        // Arrange
        var humidities = new List<Humidity>
        {
            new Humidity { ThermometerName = "Thermometer1", HumidityValue = 55.5, Timestamp = DateTime.Now }
        };

        // Act
        _displayManager.UpdateHumidityDisplay(humidities, true);

        // Assert
        _mockThermometerPresenter.Verify(p => p.UpdateDataItemsHumidity(humidities), Times.Once);
        _mockThermometerPresenter.Verify(p => p.ClearHumidityData(), Times.Never);
    }

    [Test]
    public void UpdateHumidityDisplay_WhenShowHumidityIsFalse_ThenClearHumidityData()
    {
        // Act
        _displayManager.UpdateHumidityDisplay(null, false);

        // Assert
        _mockThermometerPresenter.Verify(p => p.ClearHumidityData(), Times.Once);
        _mockThermometerPresenter.Verify(p => p.UpdateDataItemsHumidity(It.IsAny<IEnumerable<Humidity>>()), Times.Never);
    }
}
