using System.Data;

namespace Stas.Monitor.Domains;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
