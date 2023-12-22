namespace Stas.Monitor.Domains.Tests;

[TestFixture]
public class DataItemTests
{
    [Test]
    public void DataItem_ShouldInitializesPropertiesCorrectly()
    {
        // Arrange
        var timestamp = DateTime.Now;
        var difference = 5.5;
        var actualValue = 10.0;
        var measurementType = "Temperature";
        var formattedDifference = "+5.5";

        // Act
        var dataItem = new DataItem
        {
            Timestamp = timestamp,
            Difference = difference,
            ActualValue = actualValue,
            MeasurementType = measurementType,
            FormattedDifference = formattedDifference
        };

        // Assert
        Assert.AreEqual(timestamp, dataItem.Timestamp);
        Assert.AreEqual(difference, dataItem.Difference);
        Assert.AreEqual(actualValue, dataItem.ActualValue);
        Assert.AreEqual(measurementType, dataItem.MeasurementType);
        Assert.AreEqual(formattedDifference, dataItem.FormattedDifference);
    }

    [Test]
    public void GetFormattedData_ShouldReturnCorrectString()
    {
        // Arrange
        var timestamp = new DateTime(2023, 12, 5, 12, 0, 0);
        var difference = 5.5;
        var actualValue = 10.0;
        var formattedDifference = "+5.5";
        var dataItem = new DataItem
        {
            Timestamp = timestamp,
            Difference = difference,
            ActualValue = actualValue,
            FormattedDifference = formattedDifference
        };
        var expectedString = $"Timestamp: {timestamp}, Value: {actualValue}, Difference: {formattedDifference}";

        // Act
        var result = dataItem.GetFormattedData();

        // Assert
        Assert.AreEqual(expectedString, result);
    }

    [Test]
    public void IsValid_WithDifferenceLessThan10PercentOfValue_ShouldReturnFalse()
    {
        // Arrange
        var dataItem = new DataItem
        {
            ActualValue = 100.0,
            Difference = 5.0 // 5% difference
        };

        // Act
        var isValid = dataItem.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }
}

