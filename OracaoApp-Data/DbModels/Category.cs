using System.ComponentModel.DataAnnotations;

namespace OracaoApp_Data.DbModels;

public class Category
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string HexColor { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

}
