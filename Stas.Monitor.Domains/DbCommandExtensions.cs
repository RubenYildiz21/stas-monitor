using System.Data;

namespace Stas.Monitor.Domains;

public static class DbCommandExtensions
{
    public static void AddParameterWithValue(this IDbCommand command, string parameterName, object value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = parameterName;
        parameter.Value = value;
        command.Parameters.Add(parameter);
    }
}
