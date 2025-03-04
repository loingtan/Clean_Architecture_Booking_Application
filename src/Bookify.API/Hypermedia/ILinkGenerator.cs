using Asp.Versioning;
using System.Collections.Generic;

namespace Bookify.API.Hypermedia;

public interface ILinkGenerator
{
    List<LinkDto> GenerateLinks(string controllerName, object routeValues, ApiVersion apiVersion);

    List<LinkDto> GenerateCollectionLinks(
        string controllerName,
        object queryParameters,
        bool hasNext,
        bool hasPrevious,
        ApiVersion apiVersion);
}