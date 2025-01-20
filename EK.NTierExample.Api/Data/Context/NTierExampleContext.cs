using Microsoft.Data.SqlClient;
using System.Data;

namespace EK.NTierExample.Api.Data.Context;

public interface INTierExampleContext
{
    string? ConnectionString { get; }
    IDbConnection Connection { get; }
}

public class NTierExampleContext(IConfiguration configuration) : INTierExampleContext
{
    /* some people prefer to use load the connection string at registration and pass that directly
     * in as the constructor parameter instead of relying on the IConfiguration, the reason i use
     * IConfiguration is because it allows the connection string to get updated without having to
     * restart the application at the point of the GetConnectionString call, useful for when you
     * have a mistake in the connection string and you need to fix it without restarting the app
     */
    public string? ConnectionString { get; } = configuration.GetConnectionString("NTierExample") ?? throw new ArgumentNullException("invalid connectionstring");
    public IDbConnection Connection => new SqlConnection(ConnectionString);
}
