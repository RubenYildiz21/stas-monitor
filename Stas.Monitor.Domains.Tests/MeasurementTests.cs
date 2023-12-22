namespace Stas.Monitor.Domains.Tests;
[TestFixture]
public class MeasurementTests
{
    [Test]
    public void Measurement_ShouldInitializesPropertiesCorrectly()
    {
        // Arrange
        var thermometerName = "Thermometer1";
        var timestamp = DateTime.Now;
        var temperature = 22.5;
        var difference = 2.0;

        // Act
        var measurement = new Measurement
        {
            ThermometerName = thermometerName,
            Timestamp = timestamp,
            Temperature = temperature,
            Difference = difference
        };

        // Assert
        Assert.AreEqual(thermometerName, measurement.ThermometerName);
        Assert.AreEqual(timestamp, measurement.Timestamp);
        Assert.AreEqual(temperature, measurement.Temperature);
        Assert.AreEqual(difference, measurement.Difference);
        Assert.AreEqual($"{difference:F2}°C", measurement.FormattedDifference);
    }

    [Test]
    public void IsValidMeasurement_WithValidTemperature_ShouldReturnTrue()
    {
        // Arrange
        var measurement = new Measurement { Temperature = 25.0 };

        // Act
        var isValid = measurement.IsValidMeasurement();

        // Assert
        Assert.IsTrue(isValid);
    }

    [Test]
    public void IsValidMeasurement_WithInvalidTemperature_ShouldReturnFalse()
    {
        // Arrange
        var measurement = new Measurement { Temperature = -10.0 };

        // Act
        var isValid = measurement.IsValidMeasurement();

        // Assert
        Assert.IsFalse(isValid);
    }

    [Test]
    [TestCase(10.0, "Low Temperature")]
    [TestCase(50.0, "Normal Temperature")]
    [TestCase(80.0, "High Temperature")]
    public void GetHumidityAlert_ShouldReturnCorrectAlert(double temperature, string expectedAlert)
    {
        // Arrange
        var measurement = new Measurement { Temperature = temperature };

        // Act
        var alert = measurement.GetMeasurementAlert();

        // Assert
        Assert.AreEqual(expectedAlert, alert);
    }
}
