namespace Stas.Monitor.Domains;

public class IAlertServices
{
    public virtual IEnumerable<Alert> GetRecentAlerts(string thermometerName, DateTime fromTime, DateTime toTime)
    {
        throw new NotImplementedException();
    }
}