using System.Collections.Generic;

namespace Bookify.API.Hypermedia;

public abstract class LinkedResourceBase
{
    [System.Text.Json.Serialization.JsonPropertyName("_links")]
    public List<LinkDto> Links { get; set; } = new List<LinkDto>();
}