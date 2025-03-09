using Bookify.Application.Common.Mapper;
using Mapster;

namespace Bookify.API;
public static class MappingExtensions
{
    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        services.AddMapster();
        MapsterConfig.Configure();

        return services;
    }
}

