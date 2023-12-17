namespace Stas.Monitor.Domains.Tests;
[TestFixture]
public class ThermometerConfigurationTests
{
    [Test]
    public void ThermometerConfiguration_ShouldInitializesPropertiesCorrectly()
    {
        // Arrange
        var name = "Thermometer1";
        var timestampFormat = "yyyy-MM-dd HH:mm:ss";
        var tempFormat = "Celsius";
        var profile = new Dictionary<string, int> { { "alert1", 5 }, { "alert2", 10 } };

        // Act
        var thermometerConfiguration = new ThermometerConfiguration(name, timestampFormat, tempFormat, profile);

        // Assert
        Assert.AreEqual(name, thermometerConfiguration.Name);
        Assert.AreEqual(timestampFormat, thermometerConfiguration.TimestampFormat);
        Assert.AreEqual(tempFormat, thermometerConfiguration.TempFormat);
        Assert.AreEqual(profile, thermometerConfiguration.Profile);
    }
}
