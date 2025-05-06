using System.ComponentModel.DataAnnotations;

namespace Tutorial8.Models.DTOs;

public class CountryDTO
{
    public int IdCountry { get; set; }
    [Required]
    [StringLength(120)]
    public string Name { get; set; }
}