namespace Stas.Monitor.Presentations.Tests;

[TestFixture]
public class TimerManagerTests
{
    private TimerManager _timerManager;
    private bool _eventRaised;

    [SetUp]
    public void Setup()
    {
        _timerManager = new TimerManager();
        _eventRaised = false;
    }

    [Test]
    public void Timer_ElapsedEvent_ShouldBeRaised()
    {
        // Arrange
        var autoResetEvent = new AutoResetEvent(false);
        ElapsedEventHandler handler = (sender, e) =>
        {
            _eventRaised = true;
            autoResetEvent.Set();
        };

        _timerManager.StartTimer(handler);

        // Act
        autoResetEvent.WaitOne(6000); // attendre un peu plus que la durée prévue

        // Assert
        Assert.IsTrue(_eventRaised);
    }

    [Test]
    public void StopTimer_ShouldPreventElapsedEvent()
    {
        // Arrange
        var autoResetEvent = new AutoResetEvent(false);
        ElapsedEventHandler handler = (sender, e) =>
        {
            _eventRaised = true;
            autoResetEvent.Set();
        };

        _timerManager.StartTimer(handler);

        // Act
        _timerManager.StopTimer(handler);
        autoResetEvent.WaitOne(6000); // Wait a bit longer than the timer interval

        // Assert
        Assert.IsFalse(_eventRaised);
    }
}
