namespace Stas.Monitor.Domains.Tests;

[TestFixture]
public class DatabaseServiceTests
{
private Mock<ITemperatureRepository> _mockTemperatureRepository;
        private Mock<IHumidityRepository> _mockHumidityRepository;
        private DatabaseService _databaseService;

        [SetUp]
        public void Setup()
        {
            _mockTemperatureRepository = new Mock<ITemperatureRepository>();
            _mockHumidityRepository = new Mock<IHumidityRepository>();
            _databaseService = new DatabaseService(_mockTemperatureRepository.Object, _mockHumidityRepository.Object);
        }

        [Test]
        public void ShouldGetMeasurements_ReturnsCorrectData()
        {
            // Arrange
            var thermometerName = "Thermometer1";
            var fromTime = DateTime.Now.AddDays(-1);
            var expectedMeasurements = new List<Measurement> { new Measurement() };
            _mockTemperatureRepository.Setup(repo => repo.GetMeasurements(thermometerName, fromTime))
                                      .Returns(expectedMeasurements);

            // Act
            var measurements = _databaseService.GetMeasurements(thermometerName, fromTime);

            // Assert
            Assert.AreEqual(expectedMeasurements, measurements);
            _mockTemperatureRepository.Verify(repo => repo.GetMeasurements(thermometerName, fromTime), Times.Once);
        }

        [Test]
        public void ShouldGetHumidities_ReturnsCorrectData()
        {
            // Arrange
            var thermometerName = "Thermometer1";
            var fromTime = DateTime.Now.AddDays(-1);
            var expectedHumidities = new List<Humidity> { new Humidity() };
            _mockHumidityRepository.Setup(repo => repo.GetHumidities(thermometerName, fromTime))
                                   .Returns(expectedHumidities);

            // Act
            var humidities = _databaseService.GetHumidities(thermometerName, fromTime);

            // Assert
            Assert.AreEqual(expectedHumidities, humidities);
            _mockHumidityRepository.Verify(repo => repo.GetHumidities(thermometerName, fromTime), Times.Once);
        }

        [Test]
        public void ShouldGetLastMeasurementTimestamp_ReturnsCorrectTimestamp()
        {
            // Arrange
            var thermometerName = "Thermometer1";
            var expectedTimestamp = DateTime.Now.AddDays(-1);
            _mockTemperatureRepository.Setup(repo => repo.GetLastMeasurementTimestamp(thermometerName))
                                      .Returns(expectedTimestamp);

            // Act
            var timestamp = _databaseService.GetLastMeasurementTimestamp(thermometerName);

            // Assert
            Assert.AreEqual(expectedTimestamp, timestamp);
            _mockTemperatureRepository.Verify(repo => repo.GetLastMeasurementTimestamp(thermometerName), Times.Once);
        }

        [Test]
        public void GetMeasurements_WithNonExistentThermometer_ShouldReturnsEmpty()
        {
            // Arrange
            var nonExistentThermometerName = "NonExistentThermometer";
            _mockTemperatureRepository.Setup(repo => repo.GetMeasurements(nonExistentThermometerName, It.IsAny<DateTime>()))
                .Returns(new List<Measurement>());

            // Act
            var measurements = _databaseService.GetMeasurements(nonExistentThermometerName, DateTime.Now);

            // Assert
            Assert.IsEmpty(measurements);
        }

        [Test]
        public void GetMeasurements_WhenRepositoryThrowsException_ShouldThrowsException()
        {
            // Arrange
            _mockTemperatureRepository.Setup(repo => repo.GetMeasurements(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Throws(new Exception("Database error"));

            // Act & Assert
            Assert.Throws<Exception>(() => _databaseService.GetMeasurements("Thermometer1", DateTime.Now));
        }

        [Test]
        public void GetLastMeasurementTimestamp_WhenRepositoryThrowsException_ShouldThrowsException()
        {
            // Arrange
            _mockTemperatureRepository.Setup(repo => repo.GetLastMeasurementTimestamp(It.IsAny<string>()))
                .Throws(new Exception("Database error"));

            // Act & Assert
            Assert.Throws<Exception>(() => _databaseService.GetLastMeasurementTimestamp("Thermometer1"));
        }
}
