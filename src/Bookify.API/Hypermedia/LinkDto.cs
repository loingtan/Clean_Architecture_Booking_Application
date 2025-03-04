namespace Bookify.API.Hypermedia;

public class LinkDto
{
    public string Href { get; init; } = string.Empty;
    public string Rel { get; init; } = string.Empty;
    public string Method { get; init; } = string.Empty;

    public LinkDto()
    {
    }

    public LinkDto(string href, string rel, string method)
    {
        Href = href;
        Rel = rel;
        Method = method;
    }
}