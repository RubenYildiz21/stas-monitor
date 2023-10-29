namespace Stas.Monitor.Domains.Tests;

[TestFixture]
public class AlertTests
{
    [Test]
    public void ShouldSetAndRetrieveThermometerName()
    {
        // Arrange
        var alert = new Alert();
        var expectedName = "Thermometer1";

        // Act
        alert.ThermometerName = expectedName;
        var actualName = alert.ThermometerName;

        // Assert
        Assert.That(actualName, Is.EqualTo(expectedName));
    }
    
    [Test]
    public void ShouldSetAndRetrieveThermometerTimestamp()
    {
        // Arrange
        var alert = new Alert();
        var expectedTimestamp = DateTime.Now;

        // Act
        alert.Timestamp = expectedTimestamp;
        var actualTimestamp = alert.Timestamp;

        // Assert
        Assert.That(actualTimestamp, Is.EqualTo(expectedTimestamp));
    }
    
    [Test]
    public void ShouldSetAndRetrieveExpectedTemperature()
    {
        // Arrange
        var alert = new Alert();
        var expectedTemperature = 25.5;

        // Act
        alert.ExpectedTemperature = expectedTemperature;
        var actualTemperature = alert.ExpectedTemperature;

        // Assert
        Assert.That(expectedTemperature, Is.EqualTo(actualTemperature));
    }
    
    [Test]
    public void ShouldSetAndRetrieveActualTemperature()
    {
        // Arrange
        var alert = new Alert();
        var actualTemperature = 20.5;

        // Act
        alert.ActualTemperature = actualTemperature;
        var retrievedTemperature = alert.ActualTemperature;

        // Assert
        Assert.That(actualTemperature, Is.EqualTo(retrievedTemperature));
    }
}