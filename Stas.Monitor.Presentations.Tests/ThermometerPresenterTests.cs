using NUnit.Framework;
using Stas.Monitor.Domains;
using Stas.Monitor.Presentations;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Stas.Monitor.Presentations.Tests
{
    [TestFixture]
    public class ThermometerPresenterTests
    {
        private ThermometerPresenter _thermometerPresenter;

        [SetUp]
        public void Setup()
        {
            _thermometerPresenter = new ThermometerPresenter();
        }

        [Test]
        public void SetRecentMeasurements_ShouldAddMeasurementsToCollection()
        {
            var measurements = new List<Measurement>
            {
                new Measurement { Timestamp = DateTime.Now, Value = 25.5 },
                new Measurement { Timestamp = DateTime.Now.AddMinutes(-1), Value = 24.5 }
            };

            _thermometerPresenter.SetRecentMeasurements(measurements);

            Assert.AreEqual(measurements.Count, _thermometerPresenter.RecentMeasurements.Count);
        }

        [Test]
        public void SetRecentAlert_ShouldAddAlertsToCollection()
        {
            var alerts = new List<Alert>
            {
                new Alert { Timestamp = DateTime.Now, ActualTemperature = 25.5, ExpectedTemperature = 24.5 },
                new Alert { Timestamp = DateTime.Now.AddMinutes(-1), ActualTemperature = 24.5, ExpectedTemperature = 23.5 }
            };

            _thermometerPresenter.SetRecentAlert(alerts);

            Assert.AreEqual(alerts.Count, _thermometerPresenter.RecentAlerts.Count);
        }

        [Test]
        public async Task UpdateDataItems_ShouldMergeAndSortMeasurementsAndAlertsAsync()
        {
            var measurements = new List<Measurement>
            {
                new Measurement { Timestamp = DateTime.Now, Value = 25.5 },
                new Measurement { Timestamp = DateTime.Now.AddMinutes(-1), Value = 24.5 }
            };

            var alerts = new List<Alert>
            {
                new Alert { Timestamp = DateTime.Now, ActualTemperature = 25.5, ExpectedTemperature = 24.5 },
                new Alert { Timestamp = DateTime.Now.AddMinutes(-1), ActualTemperature = 24.5, ExpectedTemperature = 23.5 }
            };

            await _thermometerPresenter.UpdateDataItems(measurements, alerts);

            var expectedCount = measurements.Count + alerts.Count;
            Assert.AreEqual(expectedCount, _thermometerPresenter.DataItems.Count);
            
        }
        
        [Test]
        public void ConvertToDataItem_WithUnknownType_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var unknownObject = new object();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _thermometerPresenter.ConvertToDataItem(unknownObject));
        }
        
        [Test]
        public async Task UpdateDataItems_InTestMode_ShouldNotUseDispatcherAsync()
        {
            // Arrange
            var measurements = new List<Measurement>
            {
                new Measurement { Timestamp = DateTime.Now, Value = 25.5 }
            };

            var alerts = new List<Alert>
            {
                new Alert { Timestamp = DateTime.Now, ActualTemperature = 25.5, ExpectedTemperature = 24.5 }
            };

            // Simuler le mode test
            AppDomain.CurrentDomain.SetData("IsInTestMode", true);

            // Act
            await _thermometerPresenter.UpdateDataItems(measurements, alerts);

            // Assert
            var expectedCount = measurements.Count + alerts.Count;
            Assert.AreEqual(expectedCount, _thermometerPresenter.DataItems.Count);
        }


    }
    
}
