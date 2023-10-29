namespace Stas.Monitor.Domains.Tests;

[TestFixture]
public class DataItemTests
{
    [Test]
    public void Properties_SetValues_GetValues()
    {
        // Arrange
        var dataItem = new DataItem();
        var timestamp = DateTime.UtcNow;
        var dataType = "Measurement";
        var expectedTemperature = 20.5;
        var actualTemperature = 19.5;
        var measurementType = "Temperature";

        // Act
        dataItem.Timestamp = timestamp;
        dataItem.DataType = dataType;
        dataItem.ExpectedTemperature = expectedTemperature;
        dataItem.ActualTemperature = actualTemperature;
        dataItem.MeasurementType = measurementType;

        // Assert
        Assert.AreEqual(timestamp, dataItem.Timestamp);
        Assert.AreEqual(dataType, dataItem.DataType);
        Assert.AreEqual(expectedTemperature, dataItem.ExpectedTemperature);
        Assert.AreEqual(actualTemperature, dataItem.ActualTemperature);
        Assert.AreEqual(measurementType, dataItem.MeasurementType);
    }
        
    [Test]
    [TestCase("Alert", "#FF747B")]
    [TestCase("Measurement", "White")]
    [TestCase(null, "White")]
    [TestCase("", "White")]
    public void BackgroundColor_GivenDataType_ReturnsExpectedColor(string dataType, string expectedColor)
    {
        // Arrange
        var dataItem = new DataItem { DataType = dataType };
            
        // Act
        var backgroundColor = dataItem.BackgroundColor;

        // Assert
        Assert.AreEqual(expectedColor, backgroundColor);
    }
}