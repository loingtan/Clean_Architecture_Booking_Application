using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Entities.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Entities.Apartments;

using Dapper;
using Mapster;

namespace Bookify.Application.Apartments.GetApartment;

internal sealed class GetApartmentQueryHandler : IQueryHandler<GetApartmentQuery, ApartmentResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IApartmentRepository _apartmentRepository;

    public GetApartmentQueryHandler(
        ISqlConnectionFactory sqlConnectionFactory,
        IApartmentRepository apartmentRepository)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _apartmentRepository = apartmentRepository;
    }

    public async Task<Result<ApartmentResponse>> Handle(GetApartmentQuery query, CancellationToken cancellationToken)
    {
        // Option 1: Using Dapper for direct database query (more efficient for read operations)
        // using var connection = _sqlConnectionFactory.CreateConnection();

        // const string sql = """
        //     SELECT 
        //         a.id AS Id,
        //         a.name AS Name,
        //         a.description AS Description,
        //         a.price_amount AS Price,
        //         a.price_currency AS Currency,
        //         a.address_street AS Street,
        //         a.address_city AS City,
        //         a.address_country AS Country,
        //         a.address_zip_code AS ZipCode,
        //         a.total_rooms AS Rooms,
        //         a.total_bathrooms AS Bathrooms,
        //         a.total_beds AS Beds,
        //         a.address_coordinates_latitude AS Latitude,
        //         a.address_coordinates_longitude AS Longitude
        //     FROM apartments AS a
        //     WHERE a.id = @Id
        //     """;

        // var apartmentDto = await connection.QueryFirstOrDefaultAsync<ApartmentResponse>(
        //     sql,
        //     new { query.Id });

        // if (apartmentDto is null)
        // {
        //     return Result.Failure<ApartmentResponse>(ApartmentErrors.NotFound);
        // }

        // return apartmentDto;

        // Option 2: Using repository with specification pattern
      
        var id = new ApartmentId(query.Id);
        var apartment = await _apartmentRepository.GetByIdAsync(id, cancellationToken);

        if (apartment is null)
        {
            return Result.Failure<ApartmentResponse>(ApartmentErrors.NotFound);
        }

        var response = apartment.Adapt<ApartmentResponse>();
        
        return response;
        
    }
}