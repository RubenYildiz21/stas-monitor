namespace Stas.Monitor.Domains.Tests;

[TestFixture]
    public class MeasurementServicesTests
    {
        private const string CsvFilePath = "measurements.csv";
        private MeasurementServices _measurementServices;

        [SetUp]
        public void SetUp()
        {
            // Créez un fichier CSV de test avec des données de mesure.
            File.WriteAllLines(CsvFilePath, new[]
            {
                "ThermometerName,Timestamp,MeasurementType,Value",
                "Thermo1,01-01-2023 10:00:00,Temperature,20.5°C",
                "Thermo1,01-01-2023 11:00:00,Temperature,21.5°C",
                // ... autres lignes de données
            });
            _measurementServices = new MeasurementServices(CsvFilePath);
        }

        [TearDown]
        public void TearDown()
        {
            // Supprimez le fichier CSV de test après chaque test.
            File.Delete(CsvFilePath);
        }

        [Test]
        public void GetRecentMeasurements_ValidInput_ReturnsMeasurements()
        {
            // Arrange
            var thermometerName = "Thermo1";
            var fromTime = DateTime.ParseExact("01-01-2023 09:00:00", "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            // Act
            var measurements = _measurementServices.GetRecentMeasurements(thermometerName, fromTime);

            // Assert
            Assert.AreEqual(2, measurements.Count());  // Assume there are 2 measurements for Thermo1 after fromTime
            var measurement = measurements.First();
            Assert.AreEqual(thermometerName, measurement.ThermometerName);
            Assert.AreEqual("Temperature", measurement.MeasurementType);
            Assert.AreEqual(21.5, measurement.Value);  // Assume the data is ordered by timestamp descending
        }

        [Test]
        public void GetRecentMeasurements_FileNotFound_ThrowsException()
        {
            // Arrange
            File.Delete(CsvFilePath);  // Delete the CSV file to simulate a file not found error
            var thermometerName = "Thermo1";
            var fromTime = DateTime.ParseExact("01-01-2023 09:00:00", "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _measurementServices.GetRecentMeasurements(thermometerName, fromTime));
            Assert.IsTrue(ex.Message.Contains("Erreur lors de la lecture du fichier de mesures csv"));
        }

    }