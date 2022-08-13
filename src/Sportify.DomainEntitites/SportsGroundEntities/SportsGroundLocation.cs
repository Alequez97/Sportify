using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sportify.DomainEntities.SportsGroundEntities;

/// <summary>
/// Class that represents SportsGroundsLocation model
/// </summary>
public class SportsGroundLocation : EntityBase
{
  [MaxLength(100)]
  public string Country { get; set; }

  [MaxLength(100)]
  public string City { get; set; }

  [MaxLength(200)]
  public string District { get; set; }

  [MaxLength(200)]
  public string Street { get; set; }

  [MaxLength(100)]
  public string HouseNumber { get; set; }

  [MaxLength(500)]
  public string Description { get; set; }

  [Required]
  public double Latitude { get; set; }

  [Required]
  public double Longitude { get; set; }

  [Required]
  public int TypeId { get; set; }
  public SportsGroundType Type { get; set; }

  [Required]
  public int CreatorId { get; set; }
  public User Creator { get; set; }

  public List<SportsGroundImage> Images { get; set; }
}
