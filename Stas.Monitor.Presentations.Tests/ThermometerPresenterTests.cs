using System.Collections.ObjectModel;

namespace Stas.Monitor.Presentations.Tests;

[TestFixture]
public class ThermometerPresenterTests
{
    private Mock<IDataItemConverter> _mockDataItemConverter;
    private Mock<IUiThreadInvoker> _mockUiThreadInvoker;
    private ThermometerPresenter _thermometerPresenter;

    [SetUp]
    public void Setup()
    {
        _mockDataItemConverter = new Mock<IDataItemConverter>();
        _mockUiThreadInvoker = new Mock<IUiThreadInvoker>();
        _thermometerPresenter = new ThermometerPresenter(_mockDataItemConverter.Object, _mockUiThreadInvoker.Object);
    }

    [Test]
    public async Task UpdateDataItemsTemperature_ShouldInvokeConverterAndUiThreadInvoker()
    {
        // Arrange
        var measurements = new List<Measurement> { new Measurement() };
        var dataItems = new List<DataItem> { new DataItem() };
        _mockDataItemConverter.Setup(c => c.ConvertMeasurementsToDataItems(measurements)).Returns(dataItems);

        // Act
        await _thermometerPresenter.UpdateDataItemsTemperature(measurements);

        // Assert
        _mockDataItemConverter.Verify(c => c.ConvertMeasurementsToDataItems(measurements), Times.Once);
        _mockUiThreadInvoker.Verify(i => i.InvokeOnUIThreadAsync(It.IsAny<System.Action>()), Times.Once);
    }

    [Test]
    public async Task UpdateDataItemsHumidity_ShouldInvokeConverterAndUiThreadInvoker()
    {
        // Arrange
        var humidities = new List<Humidity> { new Humidity() };
        var dataItems = new List<DataItem> { new DataItem() };
        _mockDataItemConverter.Setup(c => c.ConvertHumiditiesToDataItems(humidities)).Returns(dataItems);

        // Act
        await _thermometerPresenter.UpdateDataItemsHumidity(humidities);

        // Assert
        _mockDataItemConverter.Verify(c => c.ConvertHumiditiesToDataItems(humidities), Times.Once);
        _mockUiThreadInvoker.Verify(i => i.InvokeOnUIThreadAsync(It.IsAny<System.Action>()), Times.Once);
    }

    [Test]
    public void ClearTemperatureData_ShouldClearDataItemsTemperature()
    {
        // Act
        _thermometerPresenter.ClearTemperatureData();

        // Assert
        Assert.IsEmpty(_thermometerPresenter.DataItemsTemperature);
    }

    [Test]
    public void ClearHumidityData_ShouldClearDataItemsHumidity()
    {
        // Act
        _thermometerPresenter.ClearHumidityData();

        // Assert
        Assert.IsEmpty(_thermometerPresenter.DataItemsHumidity);
    }

    [Test]
    public void UpdateObservableCollection_ShouldClearAndAddItems()
    {
        // Arrange
        var testCollection = new ObservableCollection<DataItem>();
        var testDataItems = new List<DataItem>
        {
            new DataItem { ActualValue = 25.0 },
            new DataItem { ActualValue = 30.0 }
        };

        // Act
        _thermometerPresenter.UpdateObservableCollection(testCollection, testDataItems);

        // Assert
        Assert.AreEqual(testDataItems.Count, testCollection.Count); // Verify collection has the correct number of items
        CollectionAssert.AreEqual(testDataItems, testCollection); // Verify the items in the collection are correct
    }
}
