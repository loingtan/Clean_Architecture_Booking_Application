using Bookify.Application.Common.Models;

namespace Bookify.Application.Apartments.SearchApartments;

public class ApartmentParameters : RequestParameters
{
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
}