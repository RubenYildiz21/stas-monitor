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
}
