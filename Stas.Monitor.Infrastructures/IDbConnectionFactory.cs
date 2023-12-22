using System.Data;

namespace Stas.Monitor.Infrastructures;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
