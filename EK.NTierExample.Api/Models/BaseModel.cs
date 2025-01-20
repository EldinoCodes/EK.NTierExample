namespace EK.NTierExample.Api.Models;

public abstract class BaseModel
{
    public string? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime ModifiedDate { get; set; }
}
