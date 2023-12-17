namespace Stas.Monitor.Domains.Tests
{
    [TestFixture]
    public class ConfigurationTests
    {
        [Test]
        public void Configuration_ShouldInitializesThermometersList()
        {
            // Arrange
            var thermometers = new List<ThermometerConfiguration>
            {
                new ThermometerConfiguration("Thermometer1", "Format1", "TempFormat1", new Dictionary<string, int>()),
                new ThermometerConfiguration("Thermometer2", "Format2", "TempFormat2", new Dictionary<string, int>())
            };

            // Act
            var configuration = new Configuration(thermometers);

            // Assert
            Assert.AreEqual(2, configuration.Thermometers.Count);
            Assert.AreEqual("Thermometer1", configuration.Thermometers[0].Name);
            Assert.AreEqual("Thermometer2", configuration.Thermometers[1].Name);
        }
    }
}
