using System.ComponentModel.DataAnnotations;

namespace Bookify.API.Controllers.Apartments;

public class CreateApartmentRequest
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int Rooms { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int Beds { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int Bathrooms { get; set; }
    
    [Required]
    public string Address { get; set; }
    
    [Required]
    public string City { get; set; }
    
    [Required]
    public string Country { get; set; }
    
    [Required]
    public decimal Latitude { get; set; }
    
    [Required]
    public decimal Longitude { get; set; }
}