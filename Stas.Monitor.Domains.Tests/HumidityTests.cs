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

    [Test]
    public void GetHumidityAlert_WithLowHumidity_ShouldReturnLowHumidityAlert()
    {
        // Arrange
        var humidity = new Humidity { HumidityValue = 20.0 };

        // Act
        var alert = humidity.GetHumidityAlert();

        // Assert
        Assert.AreEqual("Low Humidity", alert);
    }

    [Test]
    public void GetHumidityAlert_WithHighHumidity_ShouldReturnHighHumidityAlert()
    {
        // Arrange
        var humidity = new Humidity { HumidityValue = 80.0 };

        // Act
        var alert = humidity.GetHumidityAlert();

        // Assert
        Assert.AreEqual("High Humidity", alert);
    }

    [Test]
    public void GetHumidityAlert_WithNormalHumidity_ShouldReturnNormalHumidityAlert()
    {
        // Arrange
        var humidity = new Humidity { HumidityValue = 50.0 };

        // Act
        var alert = humidity.GetHumidityAlert();

        // Assert
        Assert.AreEqual("Normal Humidity", alert);
    }

    [Test]
    public void IsValidHumidity_WithHumidityWithinRange_ShouldReturnTrue()
    {
        // Arrange
        var humidity = new Humidity { HumidityValue = 50.0 };

        // Act
        var result = humidity.IsValidHumidity();

        // Assert
        Assert.IsTrue(result);
    }
}
