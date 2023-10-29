using System.Timers;
using Timer = System.Timers.Timer;

namespace Stas.Monitor.Presentations;

public class TimerManager
{
    private Timer _updateTimer;
    
    public TimerManager()
    {
        _updateTimer = new Timer(1000);
        _updateTimer.AutoReset = true;
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