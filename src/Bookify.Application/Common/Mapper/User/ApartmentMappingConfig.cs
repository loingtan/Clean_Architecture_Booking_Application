using Bookify.Domain.Entities.Apartments;
using Bookify.Domain.Shared;
using Mapster;

namespace Bookify.Application.Common.Mapper;

public static class ApartmentMappingConfig
{
    public static void Register()
    {
        TypeAdapterConfig<ApartmentId, Guid>.NewConfig()
            .MapWith(src => src.Value);
        TypeAdapterConfig<Money, Decimal>.NewConfig().MapWith(src => src.Amount);
    }
}

