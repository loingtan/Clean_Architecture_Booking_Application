using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Bookify.API.Hypermedia;

public class LinkGenerator : ILinkGenerator
{
    private readonly IUrlHelper _urlHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LinkGenerator(
        IUrlHelperFactory urlHelperFactory,
        IActionContextAccessor actionContextAccessor,
        IHttpContextAccessor httpContextAccessor)
    {
        _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        _httpContextAccessor = httpContextAccessor;
    }

    public List<LinkDto> GenerateLinks(string controllerName, object routeValues, ApiVersion apiVersion)
    {
        var links = new List<LinkDto>();

        // Add version to route values
        var routeData = new RouteValueDictionary(routeValues);
        routeData["version"] = apiVersion.ToString();

        // Get (self)
        links.Add(new LinkDto(
            _urlHelper.Link($"Get{controllerName}", routeData),
            "self",
            "GET"));

        // Update
        links.Add(new LinkDto(
            _urlHelper.Link($"Update{controllerName}", routeData),
            $"update_{controllerName.ToLowerInvariant()}",
            "PATCH"));

        // Add controller-specific links here...

        return links;
    }

    public List<LinkDto> GenerateCollectionLinks(
        string controllerName,
        object queryParameters,
        bool hasNext,
        bool hasPrevious,
        ApiVersion apiVersion)
    {
        var links = new List<LinkDto>();

        var routeData = new RouteValueDictionary(queryParameters);
        routeData["version"] = apiVersion.ToString();
        links.Add(new LinkDto(
            _urlHelper.Link($"Get{controllerName}s", routeData),
            "self",
            "GET"));
        links.Add(new LinkDto(
            _urlHelper.Link($"Create{controllerName}", new { version = apiVersion.ToString() }),
            $"create_{controllerName.ToLowerInvariant()}",
            "POST"));

        if (hasNext)
        {
            var nextParameters = new RouteValueDictionary(routeData);
            nextParameters["pageNumber"] = (int)routeData["pageNumber"] + 1;

            links.Add(new LinkDto(
                _urlHelper.Link($"Get{controllerName}s", nextParameters),
                "next_page",
                "GET"));
        }

        if (hasPrevious)
        {
            var previousParameters = new RouteValueDictionary(routeData);
            previousParameters["pageNumber"] = (int)routeData["pageNumber"] - 1;

            links.Add(new LinkDto(
                _urlHelper.Link($"Get{controllerName}s", previousParameters),
                "previous_page",
                "GET"));
        }

        return links;
    }
}