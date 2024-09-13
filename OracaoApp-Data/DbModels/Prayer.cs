using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OracaoApp.Data.DbModels;

public class Prayer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string PrayingForName { get; set; }
    public required bool IsPublic { get; set; }
    public required Guid OwnerId { get; set; }

    [ForeignKey(nameof(PrayerCategory))]
    public required int PrayerCategoryId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public virtual PrayerCategory? PrayerCategory { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public virtual List<PrayerComment>? PrayerComments { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public virtual List<PrayingFor>? PrayingFors { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
