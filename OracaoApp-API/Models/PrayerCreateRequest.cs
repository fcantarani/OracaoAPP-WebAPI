namespace OracaoApp.API.Models;

public class PrayerCreateRequest
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string PrayingForName { get; set; }
    public int CategoryId { get; set; }
}
