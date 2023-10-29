namespace Stas.Monitor.Domains.Tests;

[TestFixture]
    public class ConfigurationTests
    {
        [Test]
        public void Constructor_ValidParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var csvFilePath = "measurements.csv";
            var thermometers = new List<ThermometerConfiguration>
            {
                new ThermometerConfiguration { Name = "Thermo1" },
                new ThermometerConfiguration { Name = "Thermo2" }
            };

            // Act
            var configuration = new Configuration(csvFilePath, thermometers);

            // Assert
            Assert.AreEqual(csvFilePath, configuration.MeasurementFilePath);
            Assert.AreEqual(thermometers, configuration.Thermometers);
        }

        [Test]
        public void Constructor_NullThermometers_SetsEmptyList()
        {
            // Arrange
            var csvFilePath = "measurements.csv";

            // Act
            var configuration = new Configuration(csvFilePath, null);

            // Assert
            Assert.AreEqual(csvFilePath, configuration.MeasurementFilePath);
            Assert.IsNotNull(configuration.Thermometers);
            Assert.IsEmpty(configuration.Thermometers);
        }
        
        [Test]
        public void Properties_SetValues_GetValues()
        {
            // Arrange
            var configuration = new Configuration("initial.csv", null);
            var newThermometers = new List<ThermometerConfiguration> { new ThermometerConfiguration { Name = "NewThermo" } };
            var newMeasurementFilePath = "new_measurements.csv";
            var newAlertFilePath = "new_alerts.csv";

            // Act
            configuration.Thermometers = newThermometers;
            configuration.MeasurementFilePath = newMeasurementFilePath;
            configuration.AlertFilePath = newAlertFilePath;

            // Assert
            Assert.AreEqual(newThermometers, configuration.Thermometers);
            Assert.AreEqual(newMeasurementFilePath, configuration.MeasurementFilePath);
            Assert.AreEqual(newAlertFilePath, configuration.AlertFilePath);
        }
    }