using Dapper;
using EK.NTierExample.Api.Data.Context;
using EK.NTierExample.Api.Models.WeatherForecasts;
using Microsoft.Data.SqlClient;

namespace EK.NTierExample.Api.Data.Repositories.WeatherForecasts;

/*
 * i personally include the interface in the same file as the class that implements it unless 
 * there are multiple classes that implement the same interface using say a strategy pattern
 */
public interface IWeatherSummaryRepository
{
    Task<List<WeatherSummary>> WeatherSummaryGetAllAsync(CancellationToken cancellationToken = default);
    Task<WeatherSummary?> WeatherSummaryGetAsync(Guid? weatherSummaryId, CancellationToken cancellationToken = default);
    Task<WeatherSummary?> WeatherSummaryAddAsync(WeatherSummary? weatherSummary, CancellationToken cancellationToken = default);
    Task<bool> WeatherSummaryUpdateAsync(WeatherSummary? weatherSummary, CancellationToken cancellationToken = default);
    Task<bool> WeatherSummaryDeleteAsync(Guid? weatherSummaryId, CancellationToken cancellationToken = default);    
}

/*
 * i normally include ILogger<T> for logging purposes as i pref to log to event logs too.
 */
public class WeatherSummaryRepository(INTierExampleContext context) : IWeatherSummaryRepository
{
    private readonly INTierExampleContext _context = context;

    /*
     * this is just a simple example of a dapper repository focused on a sinlge domain entity (WeatherSummary)
     * please noted that there is no validation to cover duplicate values, etc. just basic null checks and
     * even though the methods are there only the get all method is used.
     */

    public virtual async Task<List<WeatherSummary>> WeatherSummaryGetAllAsync(CancellationToken cancellationToken = default)
    {
        var ret = new List<WeatherSummary>();

        const string sSql = @"
            SELECT WeatherSummaryId
                , CreatedBy
                , CreatedDate
                , ModifiedBy
                , ModifiedDate
                , Description
            FROM [dbo].[WeatherSummary]
        ";

        using var connection = _context.Connection;

        var sCmd = new CommandDefinition(sSql, cancellationToken: cancellationToken);
        var res = await connection.QueryAsync<WeatherSummary>(sCmd);
        if (res.Any())
            ret.AddRange(res.OrderBy(s => s.Description));

        return ret;
    }
    public virtual async Task<WeatherSummary?> WeatherSummaryGetAsync(Guid? weatherSummaryId, CancellationToken cancellationToken = default)
    {
        const string sSql = @"
            SELECT WeatherSummaryId
                , CreatedBy
                , CreatedDate
                , ModifiedBy
                , ModifiedDate
                , Description
            FROM [dbo].[WeatherSummary]
            WHERE WeatherSummaryId = @WeatherSummaryId
        ";

        using var connection = _context.Connection;

        var sParams = new { WeatherSummaryId = weatherSummaryId };
        var sCmd = new CommandDefinition(sSql, sParams, cancellationToken: cancellationToken);

        return await connection.QueryFirstOrDefaultAsync<WeatherSummary>(sCmd);
    }
    public virtual async Task<WeatherSummary?> WeatherSummaryAddAsync(WeatherSummary? weatherSummary, CancellationToken cancellationToken = default)
    {
        if (weatherSummary is null) return default;

        const string iSql = @"
            INSERT INTO [dbo].[WeatherSummary]
                ([CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [Description])
            OUTPUT INSERTED.WeatherSummaryId
            VALUES
                (GETUTCDATE(), 'SYSTEM', GETUTCDATE(), 'SYSTEM', @Description),
        ";

        using var connection = _context.Connection;
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            var iCmd = new CommandDefinition(iSql, weatherSummary, transaction, cancellationToken: cancellationToken);

            weatherSummary.WeatherSummaryId = await connection.QuerySingleAsync<Guid>(iCmd);

            transaction.Commit();
        }
        catch (SqlException ex)
        {
            transaction.Rollback();
            throw new Exception("An error occurred while adding the weather summary.", ex);
        }

        return weatherSummary;
    }
    public virtual async Task<bool> WeatherSummaryUpdateAsync(WeatherSummary? weatherSummary, CancellationToken cancellationToken = default)
    {
        var ret = false;
        if (string.IsNullOrEmpty(weatherSummary?.Description)) return ret;

        const string uSql = @"
            UPDATE [dbo].[WeatherSummary]
            SET
                [ModifiedDate] = GETUTCDATE(),
                [ModifiedBy] = 'SYSTEM',
                [Description] = @Description
            WHERE WeatherSummaryId = @WeatherSummaryId
        ";

        using var connection = _context.Connection;
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            var uCmd = new CommandDefinition(uSql, weatherSummary, transaction, cancellationToken: cancellationToken);
            ret = await connection.ExecuteAsync(uCmd) > 0;

            transaction.Commit();
        }
        catch (SqlException ex)
        {
            transaction.Rollback();
            throw new Exception("An error occurred while updating the weather summary.", ex);
        }

        return ret;
    }
    public virtual async Task<bool> WeatherSummaryDeleteAsync(Guid? weatherSummaryId, CancellationToken cancellationToken = default)
    {
        var ret = false;
        if (weatherSummaryId is null || weatherSummaryId == Guid.Empty) return ret;

        const string dSql = @"
            DELETE FROM [dbo].[WeatherSummary] WHERE WeatherSummaryId = @WeatherSummaryId
        ";


        using var connection = _context.Connection;
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            var dParams = new { WeatherSummaryId = weatherSummaryId };
            var dCmd = new CommandDefinition(dSql, dParams, transaction, cancellationToken: cancellationToken);

            ret = await connection.ExecuteAsync(dCmd) > 0;

            transaction.Commit();
        }
        catch (SqlException ex)
        {
            transaction.Rollback();
            throw new Exception("An error occurred while deleting the weather summary.", ex);
        }

        return ret;
    }
}
