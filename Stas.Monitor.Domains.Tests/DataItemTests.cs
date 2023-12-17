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
}

