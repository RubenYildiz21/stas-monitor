namespace Stas.Monitor.Infrastructures;

using System.Data;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        if (_connectionString.Contains("DataSource=:memory:")) // SQLite en mémoire
        {
            return new SqliteConnection(_connectionString);
        }
        else
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
