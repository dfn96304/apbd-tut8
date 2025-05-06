using System.ComponentModel.DataAnnotations;

namespace Tutorial8.Models.DTOs;

public class TripDTO
{
    public int IdTrip { get; set; }
    [Required]
    [StringLength(120)]
    public string Name { get; set; }
    [Required]
    [StringLength(220)]
    public string Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    [Required]
    [Range(0, int.MaxValue)]
    public int MaxPeople { get; set; }
    public List<CountryDTO> Countries { get; set; }
}

public class Client_TripDTO
{
    public int IdTrip { get; set; }
    [Required]
    [StringLength(120)]
    public string Name { get; set; }
    [Required]
    [StringLength(220)]
    public string Description { get; set; }
    [Required]
    public int RegisteredAt { get; set; }
    public int? PaymentDate { get; set; }
}