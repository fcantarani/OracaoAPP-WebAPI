using System.ComponentModel.DataAnnotations.Schema;

namespace OracaoApp.API.Models;

public class PrayingForCreateRequest
{
    public required Guid OwnerId { get; set; }

    [ForeignKey(nameof(Prayer))]
    public required int PrayerId { get; set; }
}
