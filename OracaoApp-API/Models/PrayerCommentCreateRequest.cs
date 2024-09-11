namespace OracaoApp.API.Models;

public class PrayerCommentCreateRequest
{
    public required string Message { get; set; }
    public required Guid OwnerId { get; set; }
    public required int PrayerId { get; set; }


}