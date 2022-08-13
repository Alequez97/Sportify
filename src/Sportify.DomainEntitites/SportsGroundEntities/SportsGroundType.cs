using System.ComponentModel.DataAnnotations;

namespace Sportify.DomainEntities.SportsGroundEntities;

/// <summary>
/// Class that represents SportsGroundType model
/// </summary>
public class SportsGroundType : EntityBase
{
  [Required]
  [MaxLength(100)]
  public string Name { get; set; }
}