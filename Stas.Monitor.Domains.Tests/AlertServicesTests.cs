namespace Stas.Monitor.Domains.Tests;

[TestFixture]
public class AlertServicesTests
{
    private string _tempCsvFilePath;
        
    [SetUp]
    public void SetUp()
    {
        // Création d'un fichier CSV temporaire pour les tests
        _tempCsvFilePath = Path.GetTempFileName();
        File.WriteAllLines(_tempCsvFilePath, new[]
        {
            "ThermometerName,Timestamp,ExpectedTemperature,ActualTemperature",
            "Thermo1,01-01-2022 12:00:00,20°C,25°C",
            "Thermo1,01-01-2022 13:00:00,20°C,26°C",
            // ... autres lignes ...
        });
    }

    [TearDown]
    public void TearDown()
    {
        // Suppression du fichier CSV temporaire après les tests
        File.Delete(_tempCsvFilePath);
    }
        
    [Test]
    public void GetRecentAlerts_ValidCsv_ReturnsAlerts()
    {
        // Arrange
        var alertServices = new AlertServices(_tempCsvFilePath);
            
        // Act
        var alerts = alertServices.GetRecentAlerts("Thermo1", new DateTime(2022, 1, 1, 11, 0, 0), new DateTime(2022, 1, 1, 14, 0, 0)).ToList();
            
        // Assert
        Assert.AreEqual(2, alerts.Count);
        Assert.AreEqual(new DateTime(2022, 1, 1, 13, 0, 0), alerts[0].Timestamp);
        Assert.AreEqual(26, alerts[0].ActualTemperature);
        Assert.AreEqual(20, alerts[0].ExpectedTemperature);
    }
        
    [Test]
    public void GetRecentAlerts_FileNotFound_ThrowsException()
    {
        // Arrange
        var alertServices = new AlertServices("nonexistent.csv");
            
        // Act & Assert
        var ex = Assert.Throws<Exception>(() => alertServices.GetRecentAlerts("Thermo1", DateTime.Now.AddHours(-1), DateTime.Now));
        StringAssert.Contains("Erreur lors de la lecture du fichier d'alertes csv", ex.Message);
    }
}