using EK.NTierExample.Api.Models.Addresses;
using EK.NTierExample.Api.Services.Addresses;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace EK.NTierExample.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AddressesController(ILogger<AddressesController> logger, IAddressService addressService) : ControllerBase
{
    private readonly ILogger<AddressesController> _logger = logger;
    private readonly IAddressService _addressService = addressService;

    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(Address[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAddresses(CancellationToken cancellationToken = default)
    {
        var ret = await _addressService.AddressGetAllAsync(cancellationToken);

        return Ok(ret);
    }

    [HttpGet]
    [Route("{addressId}")]
    [Consumes(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [ProducesResponseType(typeof(Address), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAddress(Guid addressId, CancellationToken cancellationToken = default)
    {
        if (addressId == Guid.Empty) return BadRequest();

        var ret = await _addressService.AddressGetAsync(addressId, cancellationToken);

        return Ok(ret);
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [ProducesResponseType(typeof(Address), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostAddress([FromBody] Address address, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var ret = await _addressService.AddressCreateAsync(address, cancellationToken);
        if (ret?.AddressId is null || ret.AddressId == Guid.Empty) return BadRequest();

        var url = $"{Request.Scheme}://{Request.Host}/Addresses/{ret.AddressId}";
        return Created(url, ret);
    }

    [HttpPut]
    [Route("{addressId}")]
    [Consumes(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutAddress(Guid addressId, [FromBody] Address address, CancellationToken cancellationToken = default)
    {
        if (addressId == Guid.Empty) return BadRequest();
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var rec = await _addressService.AddressGetAsync(addressId, cancellationToken);
        if (rec == null) return BadRequest();

        if (await TryUpdateModelAsync(rec, "", m => m.Address1, m => m.Address2, m => m.Locality, m => m.Region, m => m.PostalCode))
        {
            var ret = await _addressService.AddressUpdateAsync(rec, cancellationToken);
            if (ret)
                return Accepted(rec);
        }
        return BadRequest();
    }

    [HttpDelete]
    [Route("{addressId}")]
    [Consumes(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAddress(Guid addressId, CancellationToken cancellationToken = default)
    {
        if (addressId == Guid.Empty) return BadRequest();

        var ret = await _addressService.AddressDeleteAsync(addressId, cancellationToken);

        return ret ? Accepted() : BadRequest();
    }
}