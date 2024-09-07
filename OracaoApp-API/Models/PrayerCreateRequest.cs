namespace OracaoApp.API.Models;

public class PrayerCreateRequest
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string PrayingForName { get; set; }
    public required Boolean IsPublic{ get; set; }
    public int PrayerCategoryId { get; set; }
}
