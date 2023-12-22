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

        [Test]
        public void RemoveThermometer_ShouldRemoveThermometerFromList()
        {
            // Arrange
            var existingThermometer = new ThermometerConfiguration("ExistingThermometer", "Format", "TempFormat", new Dictionary<string, int>());
            var configuration = new Configuration(new List<ThermometerConfiguration> { existingThermometer });

            // Act
            configuration.RemoveThermometer(existingThermometer);

            // Assert
            Assert.AreEqual(0, configuration.Thermometers.Count);
        }

        [Test]
        public void RemoveThermometer_WithNonExistingThermometer_ShouldNotChangeList()
        {
            // Arrange
            var existingThermometer = new ThermometerConfiguration("ExistingThermometer", "Format", "TempFormat", new Dictionary<string, int>());
            var nonExistingThermometer = new ThermometerConfiguration("NonExistingThermometer", "Format", "TempFormat", new Dictionary<string, int>());
            var configuration = new Configuration(new List<ThermometerConfiguration> { existingThermometer });

            // Act
            configuration.RemoveThermometer(nonExistingThermometer);

            // Assert
            Assert.AreEqual(1, configuration.Thermometers.Count);
            Assert.IsTrue(configuration.Thermometers.Any(t => t == existingThermometer));
        }

        [Test]
        public void AddThermometer_ShouldAddThermometerToList()
        {
            // Arrange
            var configuration = new Configuration(new List<ThermometerConfiguration>());
            var newThermometer = new ThermometerConfiguration("NewThermometer", "Format", "TempFormat", new Dictionary<string, int>());

            // Act
            configuration.AddThermometer(newThermometer);

            // Assert
            Assert.AreEqual(1, configuration.Thermometers.Count);
            Assert.IsTrue(configuration.Thermometers.Any(t => t == newThermometer));
        }
    }
}
