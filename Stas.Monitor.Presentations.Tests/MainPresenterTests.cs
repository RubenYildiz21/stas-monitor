using Moq;
using Stas.Monitor.Domains;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Stas.Monitor.Presentations.Tests
{
    [TestFixture]
    public class MainPresenterTests
    {
        private Mock<MeasurementServices> _mockMeasurementService;
        private Mock<AlertServices> _mockAlertServices;
        private Configuration _testConfiguration;
        private MainPresenter _mainPresenter;

        [SetUp]
        public void Setup()
        {
            _mockMeasurementService = new Mock<MeasurementServices>("/Users/rubenyildiz/RiderProjects/Stas.Monitor/Stas.Monitor.Presentations.Tests/Measures.csv");
            _mockAlertServices = new Mock<AlertServices>("/Users/rubenyildiz/RiderProjects/Stas.Monitor/Stas.Monitor.Presentations.Tests/Alerts.csv");
            
            _testConfiguration = new Configuration("/Users/rubenyildiz/RiderProjects/Stas.Monitor/Stas.Monitor.Presentations.Tests/Measures.csv", new List<ThermometerConfiguration>
            {
                new ThermometerConfiguration
                {
                    Name = "Thermometer1",
                    TempFormat = "00.00°",
                    TimestampFormat = "dd-MM-yyyy HH:mm:ss"
                },
            });

            _mainPresenter = new MainPresenter(_testConfiguration, _mockMeasurementService.Object, _mockAlertServices.Object);
        }

        [Test]
        public void ShouldUpdateRecentMeasurements_ItUpdateRecentMeasurements()
        {
            var expectedMeasurements = new List<Measurement> 
            { 
                new Measurement 
                { 
                    ThermometerName = "Thermometer1",
                    Timestamp = DateTime.Now,
                    MeasurementType = "Temperature",
                    Value = 25.5
                } 
            };
            _mockMeasurementService.Setup(m => m.GetRecentMeasurements(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(expectedMeasurements);

            _mainPresenter.UpdateRecentMeasurements();

            Assert.AreEqual(expectedMeasurements, _mainPresenter.RecentMeasurements);
        }

        [Test]
        public void ShouldUpdateRecentAlerts_ItUpdateRecentAlerts()
        {
            _mainPresenter.UpdateRecentMeasurements();
            _mainPresenter.UpdateRecentAlerts();
            // Arrange
            var expectedAlerts = new List<Alert> 
            { 
                new Alert 
                { 
                    ThermometerName = "Thermometer1",
                    Timestamp = DateTime.Now,
                    ExpectedTemperature = 25.0,
                    ActualTemperature = 26.5
                } 
            };
            
            _mockAlertServices.Setup(a => a.GetRecentAlerts(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(expectedAlerts);

            // Act
            _mainPresenter.UpdateRecentAlerts();

            // Assert
            Assert.AreEqual(expectedAlerts, _mainPresenter.RecentAlerts);
            _mockAlertServices.Verify(a => a.GetRecentAlerts(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }




        [Test]
        public void SetCurrentThermometer_ShouldUpdateCurrentThermometer()
        {
            var thermometerName = "Thermometer1";
            _mainPresenter.SetCurrentThermometer(thermometerName);

            Assert.AreEqual(thermometerName, _mainPresenter.CurrentThermometer.Name);
        }

        [Test]
        public void OnThermometerSelectionChanged_ShouldUpdateCurrentThermometer()
        {
            var thermometerName = "Thermometer1";
            var selectedThermometer = new ThermometerConfiguration { Name = thermometerName };

            var routedEvent = Avalonia.Interactivity.RoutedEvent.Register<MainPresenter, SelectionChangedEventArgs>("SelectionChanged", RoutingStrategies.Bubble);
            _mainPresenter.OnThermometerSelectionChanged(this, new SelectionChangedEventArgs(routedEvent, new List<object>(), new List<object> { selectedThermometer }));

            Assert.AreEqual(thermometerName, _mainPresenter.CurrentThermometer.Name);
        }
        
        
        [Test]
        public void UpdateRecentMeasurements_WhenNoMeasurements_ShouldHandleGracefully()
        {
            _mockMeasurementService.Setup(m => m.GetRecentMeasurements(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(new List<Measurement>());

            _mainPresenter.UpdateRecentMeasurements();

            Assert.IsEmpty(_mainPresenter.RecentMeasurements);
        }
        
        [Test]
        public void UpdateRecentMeasurements_ShouldCallServiceWithCorrectThermometerName()
        {
            var thermometerName = "Thermometer1";
            _mainPresenter.SetCurrentThermometer(thermometerName);

            _mainPresenter.UpdateRecentMeasurements();

            _mockMeasurementService.Verify(m => m.GetRecentMeasurements("Thermometer1", It.IsAny<DateTime>()), Times.Exactly(2));
        }



    }
}
