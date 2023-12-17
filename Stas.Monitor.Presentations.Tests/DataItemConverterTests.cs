namespace Stas.Monitor.Presentations.Tests;

[TestFixture]
public class DataItemConverterTests
{
    [Test]
    public void ConvertMeasurementsToDataItems_ShouldConvertCorrectly()
    {
        // Arrange
        var measurements = new List<Measurement>
        {
            new Measurement { ThermometerName = "Thermometer1", Temperature = 25.5, Timestamp = DateTime.Now, Difference = 1.5 },
            new Measurement { ThermometerName = "Thermometer2", Temperature = 22.0, Timestamp = DateTime.Now.AddMinutes(-10), Difference = -0.5 }
        };
        var converter = new DataItemConverter();

        // Act
        var result = converter.ConvertMeasurementsToDataItems(measurements);

        // Assert
        Assert.AreEqual(2, result.Count());
        foreach (var dataItem in result)
        {
            var measurement = measurements.FirstOrDefault(m => m.Timestamp == dataItem.Timestamp);
            Assert.IsNotNull(measurement);
            Assert.AreEqual(measurement.Temperature, dataItem.ActualValue);
            Assert.AreEqual(measurement.Difference, dataItem.Difference);
            Assert.AreEqual(measurement.FormattedDifference, dataItem.FormattedDifference);
        }
    }

    [Test]
    public void ConvertHumiditiesToDataItems_ShouldConvertCorrectly()
    {
        // Arrange
        var humidities = new List<Humidity>
        {
            new Humidity { ThermometerName = "Thermometer1", HumidityValue = 55.5, Timestamp = DateTime.Now, Difference = 5.5 },
            new Humidity { ThermometerName = "Thermometer2", HumidityValue = 60.0, Timestamp = DateTime.Now.AddMinutes(-10), Difference = -4.0 }
        };
        var converter = new DataItemConverter();

        // Act
        var result = converter.ConvertHumiditiesToDataItems(humidities);

        // Assert
        Assert.AreEqual(2, result.Count());
        foreach (var dataItem in result)
        {
            var humidity = humidities.FirstOrDefault(h => h.Timestamp == dataItem.Timestamp);
            Assert.IsNotNull(humidity);
            Assert.AreEqual(humidity.HumidityValue, dataItem.ActualValue);
            Assert.AreEqual(humidity.Difference, dataItem.Difference);
            Assert.AreEqual(humidity.FormattedDifference, dataItem.FormattedDifference);
        }
    }
}
