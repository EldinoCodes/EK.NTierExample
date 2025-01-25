using Dapper;
using EK.NTierExample.Api.Data.Context;
using EK.NTierExample.Api.Models.Addresses;
using Microsoft.Data.SqlClient;

namespace EK.NTierExample.Api.Data.Repositories.Addresses;

public interface IAddressRepository
{
    Task<List<Address>> AddressGetAllAsync(CancellationToken cancellationToken = default);
    Task<Address?> AddressGetAsync(Guid addressId, CancellationToken cancellationToken = default);
    Task<Address?> AddressCreateAsync(Address? address, CancellationToken cancellationToken = default);
    Task<bool> AddressUpdateAsync(Address? address, CancellationToken cancellationToken = default);
    Task<bool> AddressDeleteAsync(Guid addressId, CancellationToken cancellationToken = default);
}

public class AddressRepository(ILogger<AddressRepository> logger, INTierExampleContext nTierExampleContext) : IAddressRepository
{
    private readonly ILogger<AddressRepository> _logger = logger;
    private readonly INTierExampleContext _nTierExampleContext = nTierExampleContext;

    public async Task<List<Address>> AddressGetAllAsync(CancellationToken cancellationToken = default)
    {
        var ret = new List<Address>();

        const string sSql = @"
            SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
            SELECT AddressId, Address1, Address2, Locality, Region, PostalCode, Country, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate 
            FROM dbo.Address WITH (NOLOCK);
        ";

        using var connection = _nTierExampleContext.Connection;

        var sCmd = new CommandDefinition(sSql, null, cancellationToken: cancellationToken);
        var res = await connection.QueryAsync<Address>(sCmd);
        if (res.Any())
            ret.AddRange(res);

        return ret;
    }
    public async Task<Address?> AddressGetAsync(Guid addressId, CancellationToken cancellationToken = default)
    {
        if (addressId == Guid.Empty) return default;

        const string sSql = @"
            SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
            SELECT TOP 1
                AddressId, Address1, Address2, Locality, Region, PostalCode, Country, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate 
            FROM dbo.Address WITH (NOLOCK) 
            WHERE AddressId = @addressId;
        ";

        using var connection = _nTierExampleContext.Connection;
        var param = new { AddressId = addressId };
        var sCmd = new CommandDefinition(sSql, param, cancellationToken: cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<Address>(sCmd);
    }
    public async Task<Address?> AddressCreateAsync(Address? address, CancellationToken cancellationToken = default)
    {
        if (address is null) return default;

        const string iSql = @"
            INSERT INTO dbo.Address
                (Address1, Address2, Locality, Region, PostalCode, Country, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate)
            OUTPUT INSERTED.AddressId
            VALUES
                (@Address1, @Address2, @Locality, @Region, @PostalCode, @Country, @CreatedBy, GETUTCDATE(), @ModifiedBy, GETUTCDATE());
        ";

        using var connection = _nTierExampleContext.Connection;
        
        connection.Open();

        using var transaction = connection.BeginTransaction();
        
        try
        {
            var iCmd = new CommandDefinition(iSql, address, transaction, cancellationToken: cancellationToken);

            address.AddressId = await connection.QuerySingleAsync<Guid>(iCmd);

            transaction.Commit();
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "Unable to create address");

            transaction.Rollback();            
        }        

        connection.Close();
        
        return address;
    }
    public async Task<bool> AddressUpdateAsync(Address? address, CancellationToken cancellationToken = default)
    {
        var ret = false;

        if (address is null) return ret;

        const string uSql = @"
            UPDATE dbo.Address
            SET
                Address1 = @Address1,
                Address2 = @Address2,
                Locality = @Locality,
                Region = @Region,
                PostalCode = @PostalCode,
                Country = @Country,
                ModifiedBy = @ModifiedBy,
                ModifiedDate = GETUTCDATE()
            WHERE AddressId = @AddressId;
        ";

        using var connection = _nTierExampleContext.Connection;
        
        connection.Open();

        using var transaction = connection.BeginTransaction();
        
        try
        {
            var uCmd = new CommandDefinition(uSql, address, transaction, cancellationToken: cancellationToken);

            ret = await connection.ExecuteAsync(uCmd) > 0;

            transaction.Commit();
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "Unable to update address");

            transaction.Rollback();            
        }
        

        connection.Close();
        
        return ret;
    }
    public async Task<bool> AddressDeleteAsync(Guid addressId, CancellationToken cancellationToken = default)
    {
        var ret = false;

        if (addressId == Guid.Empty) return ret;

        const string dSql = @"
            DELETE FROM dbo.Address WHERE AddressId = @AddressId; 
        ";

        using var connection = _nTierExampleContext.Connection;
        
        connection.Open();

        using var transaction = connection.BeginTransaction();
        
        try
        {
            var param = new { AddressId = addressId };
            var dCmd = new CommandDefinition(dSql, param, transaction, cancellationToken: cancellationToken);
            ret = await connection.ExecuteAsync(dCmd) > 0;

            transaction.Commit();
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "Unable to delete address");

            transaction.Rollback();            
        }        

        connection.Close();
        
        return ret;
    }    
}