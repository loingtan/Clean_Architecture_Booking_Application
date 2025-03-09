namespace Bookify.Application.Common.Mapper;
public class MapsterConfig
{
    public static void Configure()
    {
        ApartmentMappingConfig.Register();
        UserMappingConfig.Register();
    }

}

