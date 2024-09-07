namespace OracaoApp.API.Models;

public class PrayerCommentCreateRequest
{
    public required string Message { get; set; }
    public required string Owner { get; set; }
    public required int PrayerId { get; set; }


}