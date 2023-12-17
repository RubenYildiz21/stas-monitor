namespace Stas.Monitor.Domains.Tests;
[TestFixture]
public class DbCommandExtensionsTests
{
    [Test]
    public void AddParameterWithValue_ShouldAddParameterToCommand()
    {
        // Arrange
        var mockCommand = new Mock<IDbCommand>();
        var mockParameter = new Mock<IDbDataParameter>();
        var mockParameterCollection = new Mock<IDataParameterCollection>();
        mockCommand.Setup(c => c.CreateParameter()).Returns(mockParameter.Object);
        mockCommand.Setup(c => c.Parameters).Returns(mockParameterCollection.Object);
        var command = mockCommand.Object;
        var parameterName = "@testParam";
        var parameterValue = "testValue";

        // Act
        DbCommandExtensions.AddParameterWithValue(command, parameterName, parameterValue);

        // Assert
        mockCommand.Verify(c => c.CreateParameter(), Times.Once);
        mockParameterCollection.Verify(p => p.Add(It.IsAny<IDbDataParameter>()), Times.Once);
    }
}
