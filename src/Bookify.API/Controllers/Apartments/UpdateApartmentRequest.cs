namespace Bookify.API.Controllers.Apartments;

public class UpdateApartmentRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? Rooms { get; set; }
    public int? Beds { get; set; }
    public int? Bathrooms { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}