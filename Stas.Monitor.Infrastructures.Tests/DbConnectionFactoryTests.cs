using MySql.Data.MySqlClient;

namespace Stas.Monitor.Infrastructures.Tests;
[TestFixture]
public class DbConnectionFactoryTests
{

    [Test]
    public void ShouldCreateConnection_ReturnsMySqlConnection()
    {
        // Arrange
        var connectionString = "Server=192.168.132.200;Database=E200382;User=E200382;Password=0382;Port=13306;";
        var factory = new DbConnectionFactory(connectionString);

        // Act
        var connection = factory.CreateConnection();

        // Assert
        Assert.IsNotNull(connection);
        Assert.IsInstanceOf<MySqlConnection>(connection);
    }

}
