using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Stas.Monitor.Presentations.Tests;

[TestFixture]
public class MainPresenterTests
{
    private Mock<IDisplayManager> _mockDisplayManager;
    private Mock<IMeasurementServices> _mockMeasurementServices;
    private Mock<IThermometerPresenter> _mockThermometerPresenter;
    private Configuration _testConfiguration;
    private MainPresenter _mainPresenter;

    [SetUp]
    public void Setup()
    {
        _mockDisplayManager = new Mock<IDisplayManager>();
        _mockMeasurementServices = new Mock<IMeasurementServices>();
        _mockThermometerPresenter = new Mock<IThermometerPresenter>();
        _testConfiguration = new Configuration(new List<ThermometerConfiguration>
        {
            new ThermometerConfiguration("TestThermometer", "dd-MM-yyyy HH:mm:ss", "00.00°C", new Dictionary<string, int>()),
            new ThermometerConfiguration("NewThermometer", "dd-MM-yyyy HH:mm:ss", "00.00°C", new Dictionary<string, int>())
        });

        _mockMeasurementServices.Setup(m => m.GetRecentMeasurements(It.IsAny<string>(), It.IsAny<DateTime>()))
                        .Returns(new List<Measurement>());
        _mockMeasurementServices.Setup(m => m.GetRecentHumidities(It.IsAny<string>(), It.IsAny<DateTime>()))
                        .Returns(new List<Humidity>());

        _mainPresenter = new MainPresenter(_testConfiguration, _mockMeasurementServices.Object);
    }

    [Test]
    public void Constructor_ShouldInitializesPropertiesCorrectly()
    {
        Assert.AreEqual(_testConfiguration, _mainPresenter.Configuration);
        Assert.IsNotNull(_mainPresenter.SelectedThermometer);
        Assert.AreEqual("1 minute", _mainPresenter.SelectedDuration);
    }

    [Test]
    public void SetCurrentThermometer_ShouldUpdatesCurrentThermometer()
    {
        var newThermometerName = "TestThermometer";
        _mainPresenter.SetCurrentThermometer(newThermometerName);
        Assert.AreEqual(newThermometerName, _mainPresenter.CurrentThermometer.Name);
    }

    [Test]
    public void UpdateRecentMeasurements_ShouldUpdatesMeasurementsProperty()
    {
        // Act
        _mainPresenter.UpdateRecentMeasurements();

        // Assert
        Assert.IsNotNull(_mainPresenter.RecentMeasurements);
    }

    [Test]
    public void UpdateRecentHumidities_ShouldUpdatesHumiditiesProperty()
    {
        // Act
        _mainPresenter.UpdateRecentHumidities();

        // Assert
        Assert.IsNotNull(_mainPresenter.RecentHumidities);
    }

    [Test]
    public void UpdateDisplayBasedOnDuration_ShouldUpdatesMeasurementsAndHumidities()
    {
        // Arrange
        var expectedMeasurements = new List<Measurement> { new Measurement() };
        var expectedHumidities = new List<Humidity> { new Humidity() };
        _mockMeasurementServices.Setup(m => m.GetRecentMeasurements(It.IsAny<string>(), It.IsAny<DateTime>()))
            .Returns(expectedMeasurements);
        _mockMeasurementServices.Setup(m => m.GetRecentHumidities(It.IsAny<string>(), It.IsAny<DateTime>()))
            .Returns(expectedHumidities);

        // Act
        _mainPresenter.SelectedDuration = "1 minute"; // Change duration to trigger update
        _mainPresenter.UpdateDisplayBasedOnDuration();

        // Assert
        Assert.AreEqual(expectedMeasurements, _mainPresenter.RecentMeasurements);
        Assert.AreEqual(expectedHumidities, _mainPresenter.RecentHumidities);
    }

    [Test]
    public void OnThermometerSelectionChanged_ShouldUpdatesCurrentThermometer()
    {
        // Arrange
        var newThermometer = new ThermometerConfiguration("TestThermometer", "dd-MM-yyyy HH:mm:ss", "00.00°C", new Dictionary<string, int>());
        var addedItems = new List<object> { newThermometer };
        var selectionChangedEventArgs = new SelectionChangedEventArgs(null, addedItems, null);

        // Act
        _mainPresenter.OnThermometerSelectionChanged(this, selectionChangedEventArgs);

        // Assert
        Assert.AreEqual(newThermometer.Name, _mainPresenter.CurrentThermometer.Name);
        Assert.AreEqual(newThermometer.TimestampFormat, _mainPresenter.CurrentThermometer.TimestampFormat);
        Assert.AreEqual(newThermometer.TempFormat, _mainPresenter.CurrentThermometer.TempFormat);
    }
}
