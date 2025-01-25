using System.ComponentModel.DataAnnotations;

namespace EK.NTierExample.Api.Models.Addresses;

public class Address : BaseModel
{
    public Guid? AddressId { get; set; }
    [Required]
    public virtual string? Address1 { get; set; }
    public virtual string? Address2 { get; set; }
    [Required]
    public virtual string? Locality { get; set; }
    [Required]
    public virtual string? Region { get; set; }

    [Required]
    [MinLength(1), MaxLength(100)]
    public virtual string? PostalCode { get; set; }
    [Required]
    public virtual string? Country { get; set; }
}
