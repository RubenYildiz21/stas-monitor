namespace Stas.Monitor.Domains.Tests;

[TestFixture]
public class MeasurementTests
{
    [Test]
    public void Properties_SetValues_GetValues()
    {
        // Arrange
        var measurement = new Measurement();
        var thermometerName = "Thermo1";
        var timestamp = DateTime.UtcNow;
        var measurementType = "Temperature";
        var value = 20.5;

        // Act
        measurement.ThermometerName = thermometerName;
        measurement.Timestamp = timestamp;
        measurement.MeasurementType = measurementType;
        measurement.Value = value;

        // Assert
        Assert.AreEqual(thermometerName, measurement.ThermometerName);
        Assert.AreEqual(timestamp, measurement.Timestamp);
        Assert.AreEqual("Measurement", measurement.DataType);  // DataType is a fixed value
        Assert.AreEqual(measurementType, measurement.MeasurementType);
        Assert.AreEqual(value, measurement.Value);
    }
        
    [Test]
    public void DataType_Set_ThrowsNotImplementedException()
    {
        // Arrange
        var measurement = new Measurement();

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => measurement.DataType = "NewType");
    }
        
    [Test]
    public void InheritedProperties_SetValues_GetValues()
    {
        // Arrange
        var measurement = new Measurement();
        var expectedTemperature = 20.5;
        var actualTemperature = 19.5;

        // Act
        measurement.ExpectedTemperature = expectedTemperature;
        measurement.ActualTemperature = actualTemperature;

        // Assert
        Assert.AreEqual(expectedTemperature, measurement.ExpectedTemperature);
        Assert.AreEqual(actualTemperature, measurement.ActualTemperature);
    }
}