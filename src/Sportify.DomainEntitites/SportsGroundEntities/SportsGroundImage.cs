using System.ComponentModel.DataAnnotations;

namespace Sportify.DomainEntities.SportsGroundEntities;

/// <summary>
/// Class that represents SportsGroundImage model
/// </summary>
public class SportsGroundImage : EntityBase
{
  [Required]
  public string Path { get; set; }

  [Required]
  public int SportsGroundLocationId { get; set; }
  public SportsGroundLocation SportsGroundLocation { get; set; }
}
