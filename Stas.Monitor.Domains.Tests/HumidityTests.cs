namespace Stas.Monitor.Domains.Tests;
[TestFixture]
public class HumidityTests
{
    [Test]
    public void Humidity_ShouldInitializesPropertiesCorrectly()
    {
        // Arrange
        var thermometerName = "Thermometer1";
        var timestamp = DateTime.Now;
        var humidityValue = 45.0;
        var difference = 5.0;

        // Act
        var humidity = new Humidity
        {
            ThermometerName = thermometerName,
            Timestamp = timestamp,
            HumidityValue = humidityValue,
            Difference = difference
        };

        // Assert
        Assert.AreEqual(thermometerName, humidity.ThermometerName);
        Assert.AreEqual(timestamp, humidity.Timestamp);
        Assert.AreEqual(humidityValue, humidity.HumidityValue);
        Assert.AreEqual(difference, humidity.Difference);
        Assert.AreEqual($"{difference:F2}%", humidity.FormattedDifference);
    }
}
