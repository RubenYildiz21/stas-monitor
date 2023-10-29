namespace Stas.Monitor.Domains.Tests;

[TestFixture]
public class ThermometerConfigurationTests
{
    private ThermometerConfiguration _thermometerConfiguration;

    [SetUp]
    public void SetUp()
    {
        _thermometerConfiguration = new ThermometerConfiguration
        {
            Name = "Thermo1",
            TimestampFormat = "dd-MM-yyyy HH:mm:ss",
            TempFormat = "°C",
            Profile = new Dictionary<string, int>
            {
                {"Profile1", 10},
                {"Profile2", 20}
            }
        };
    }

    [Test]
    public void Properties_SetAndRetrieved_Correctly()
    {
        // Assert
        Assert.AreEqual("Thermo1", _thermometerConfiguration.Name);
        Assert.AreEqual("dd-MM-yyyy HH:mm:ss", _thermometerConfiguration.TimestampFormat);
        Assert.AreEqual("°C", _thermometerConfiguration.TempFormat);
        Assert.AreEqual(2, _thermometerConfiguration.Profile.Count);
        Assert.AreEqual(10, _thermometerConfiguration.Profile["Profile1"]);
        Assert.AreEqual(20, _thermometerConfiguration.Profile["Profile2"]);
    }
        
    [Test]
    public void ProfileDictionary_AddAndRemoveEntries_WorkCorrectly()
    {
        // Act
        _thermometerConfiguration.Profile.Add("Profile3", 30);
        _thermometerConfiguration.Profile.Remove("Profile1");

        // Assert
        Assert.AreEqual(2, _thermometerConfiguration.Profile.Count);
        Assert.AreEqual(30, _thermometerConfiguration.Profile["Profile3"]);
        Assert.IsFalse(_thermometerConfiguration.Profile.ContainsKey("Profile1"));
    }
}