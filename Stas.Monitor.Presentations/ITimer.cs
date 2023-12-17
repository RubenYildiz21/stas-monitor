using System.Timers;

namespace Stas.Monitor.Presentations;

public interface ITimer
{
    void Start();

    void Stop();

    event ElapsedEventHandler Elapsed;

    double Interval { get; set; }

    bool AutoReset { get; set; }
}
