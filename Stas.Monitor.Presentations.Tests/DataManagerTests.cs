using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Serilog;
using Stas.Monitor.Domains;
using Stas.Monitor.Presentations;

namespace Stas.Monitor.Presentations.Tests
{
    [TestFixture]
    public class DataManagerTests
    {
        private Mock<MeasurementServices> _mockMeasurementService;
        private Mock<AlertServices> _mockAlertServices;
        private DataManager _dataManager;

        [SetUp]
        public void Setup()
        {
            _mockMeasurementService = new Mock<MeasurementServices>();
            _mockAlertServices = new Mock<AlertServices>();
            var thermometers = new List<ThermometerConfiguration>
            {
                new ThermometerConfiguration
                {
                    Name = "Thermometer2",
                    TempFormat = "00.00°",
                    TimestampFormat = "dd-MM-yyyy HH:mm:ss"
                },
            };
            var config = new Configuration("/Users/rubenyildiz/RiderProjects/Stas.Monitor/Stas.Monitor.Presentations.Tests/Measures.csv", thermometers);
            var thermometerConfig = new ThermometerConfiguration { Name = "Thermometer1" };
            _dataManager = new DataManager(config, _mockMeasurementService.Object, _mockAlertServices.Object, thermometerConfig);
        }
        
        
        [Test]
        public void GetRecentAlerts_ValidRecentMeasurements_ReturnsAlerts()
        {
            // Arrange
            var measurements = new List<Measurement>
            {
                new Measurement
                {
                    ThermometerName = "Thermometer1",
                    Timestamp = DateTime.Now.AddMinutes(-5),
                    MeasurementType = "Temperature",
                    Value = 25.5
                },
                new Measurement
                {
                    ThermometerName = "Thermometer1",
                    Timestamp = DateTime.Now,
                    MeasurementType = "Temperature",
                    Value = 30.5
                }
            };
            var expectedAlerts = new List<Alert>
            {
                new Alert
                {
                    ThermometerName = "Thermometer1",
                    Timestamp = DateTime.Now,
                    ExpectedTemperature = 25.5,
                    ActualTemperature = 30.5
                }
            };
            _mockAlertServices.Setup(a => a.GetRecentAlerts(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(expectedAlerts);

            // Act
            var result = _dataManager.GetRecentAlerts(measurements);

            // Assert
            Assert.AreEqual(expectedAlerts, result.ToList());
        }


        [Test]
        public void GetRecentAlerts_ValidData_ReturnsAlerts()
        {
            // Arrange
            var measurements = new List<Measurement>
            {
                new Measurement
                {
                    ThermometerName = "Thermometer1",
                    Timestamp = DateTime.Now.AddMinutes(-5),
                    MeasurementType = "Temperature",
                    Value = 25.5
                },
                new Measurement
                {
                    ThermometerName = "Thermometer1",
                    Timestamp = DateTime.Now,
                    MeasurementType = "Temperature",
                    Value = 30.5
                }
            };
            var expectedAlerts = new List<Alert>
            {
                new Alert
                {
                    ThermometerName = "Thermometer1",
                    Timestamp = DateTime.Now,
                    ExpectedTemperature = 25.5,
                    ActualTemperature = 30.5
                }
            };
            _mockAlertServices.Setup(a => a.GetRecentAlerts(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(expectedAlerts);

            // Act
            var result = _dataManager.GetRecentAlerts(measurements);

            // Assert
            Assert.AreEqual(expectedAlerts, result);
        }

        [Test]
        public void GetRecentMeasurements_NoMatchingThermometerName_ThrowsException()
        {
            // Arrange
            var validCsvContent = new[] { "Thermometer2,25-10-2023 12:00:00,25.5°C" };
            
            File.AppendAllLines(_dataManager.Configuration.MeasurementFilePath, validCsvContent);
            
            // Act & Assert
            Assert.Throws<FormatException>(() => _dataManager.GetRecentMeasurements());
        }
        
        [Test]
        public void SetCurrentThermometer_ThermometerNotFound_LogsError()
        {
            // Arrange
            var invalidThermometerName = "InvalidThermometer";

            // Act
            Assert.Throws<NSubstitute.Exceptions.ArgumentNotFoundException>(() => _dataManager.SetCurrentThermometer(invalidThermometerName));            
        }

        [Test]
        public void GetRecentAlerts_EmptyRecentMeasurements_ReturnsEmptyList()
        {
            // Arrange
            var measurements = Enumerable.Empty<Measurement>();

            // Act
            var result = _dataManager.GetRecentAlerts(measurements);

            // Assert
            Assert.IsEmpty(result);
        }


    }
}
