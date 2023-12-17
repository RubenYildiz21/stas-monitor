using System.Timers;

namespace Stas.Monitor.Presentations;

public class TimerManager
{
    private ITimer _updateTimer;

    public TimerManager()
    {
        _updateTimer = new TimerWrapper(5000) { AutoReset = true };
    }

    public void StartTimer(ElapsedEventHandler onElapsed)
    {
        _updateTimer.Elapsed += onElapsed;
        _updateTimer.Start();
    }

    public void StopTimer(ElapsedEventHandler onElapsed)
    {
        _updateTimer.Elapsed -= onElapsed;
        _updateTimer.Stop();
    }
}
