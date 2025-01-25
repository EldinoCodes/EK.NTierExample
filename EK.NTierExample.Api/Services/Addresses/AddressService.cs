using EK.NTierExample.Api.Data.Repositories.Addresses;
using EK.NTierExample.Api.Models.Addresses;
using System.Security.Principal;

namespace EK.NTierExample.Api.Services.Addresses;

public interface IAddressService
{
    Task<List<Address>> AddressGetAllAsync(CancellationToken cancellationToken = default);
    Task<Address?> AddressGetAsync(Guid addressId, CancellationToken cancellationToken = default);
    Task<Address?> AddressCreateAsync(Address address, CancellationToken cancellationToken = default);
    Task<bool> AddressUpdateAsync(Address address, CancellationToken cancellationToken = default);
    Task<bool> AddressDeleteAsync(Guid addressId, CancellationToken cancellationToken = default);
}

public class AddressService(ILogger<AddressService> logger, IPrincipal principal, IAddressRepository addressesRepository) : IAddressService
{
    private readonly ILogger<AddressService> _logger = logger;
    private readonly IPrincipal _principal = principal;
    private readonly IAddressRepository _addressesRepository = addressesRepository;

    public virtual async Task<List<Address>> AddressGetAllAsync(CancellationToken cancellationToken = default)
        => await _addressesRepository.AddressGetAllAsync(cancellationToken);
    public virtual async Task<Address?> AddressGetAsync(Guid addressId, CancellationToken cancellationToken = default) 
        => await _addressesRepository.AddressGetAsync(addressId, cancellationToken);
    public virtual async Task<Address?> AddressCreateAsync(Address? address, CancellationToken cancellationToken = default)
    {
        if (address is null) return default;

        var userName = _principal?.Identity?.Name ?? "Unknown";

        address.CreatedBy = userName;
        address.ModifiedBy = userName;

        return await _addressesRepository.AddressCreateAsync(address, cancellationToken);
    }
    public virtual async Task<bool> AddressUpdateAsync(Address? address, CancellationToken cancellationToken = default)
    {
        if (address is null) return false;

        var userName = _principal?.Identity?.Name ?? "Unknown";

        address.ModifiedBy = userName;

        return await _addressesRepository.AddressUpdateAsync(address, cancellationToken);
    }
    public virtual async Task<bool> AddressDeleteAsync(Guid addressId, CancellationToken cancellationToken = default)
    {
        return addressId != Guid.Empty
            ? await _addressesRepository.AddressDeleteAsync(addressId, cancellationToken)
            : false;
    }
}