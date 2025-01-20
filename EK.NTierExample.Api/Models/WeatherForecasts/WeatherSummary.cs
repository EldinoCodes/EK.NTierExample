using System.ComponentModel.DataAnnotations;

namespace EK.NTierExample.Api.Models.WeatherForecasts;

/*
 * this is a simple domain model class that represents a weather summary
 * largely just managed this way to display full CRUD operations against DAL (Data Access Layer)
 */
public class WeatherSummary : BaseModel
{
    public Guid? WeatherSummaryId { get; set; }
    [StringLength(128, MinimumLength = 1)]
    public string? Description { get; set; }
}
