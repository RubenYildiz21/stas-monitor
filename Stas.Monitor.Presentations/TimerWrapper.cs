using System.Timers;
using Timer = System.Timers.Timer;

namespace Stas.Monitor.Presentations;

public class TimerWrapper : ITimer
{
    private readonly Timer _timer;

    public TimerWrapper(double interval)
    {
        _timer = new Timer(interval);
    }

    public event ElapsedEventHandler Elapsed
    {
        add { _timer.Elapsed += value; }
        remove { _timer.Elapsed -= value; }
    }

    public double Interval
    {
        get => _timer.Interval;
        set => _timer.Interval = value;
    }

    public bool AutoReset
    {
        get => _timer.AutoReset;
        set => _timer.AutoReset = value;
    }

    public void Start() => _timer.Start();

    public void Stop() => _timer.Stop();
}
